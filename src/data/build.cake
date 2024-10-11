//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#load "./build/parameters.cake"

#tool "nuget:?package=GitVersion.CommandLine&version=5.1.3"
#tool "nuget:?package=MSBuild.Extension.Pack&version=1.9.1"
#tool "nuget:?package=DLaB.Xrm.EarlyBoundGenerator.Api&version=1.2019.5.17"
#tool "nuget:?package=SqlPackage.CommandLine&version=14.0.3953.4"

#addin "Cake.Issues&version=0.8.1"
#addin "Cake.Issues.Reporting&version=0.8.0"
#addin "Cake.Issues.MsBuild&version=0.8.0"
#addin "Cake.AzureDevOps&version=0.4.4"
#addin "Cake.Issues.PullRequests&version=0.8.0"
#addin "Cake.Issues.PullRequests.AzureDevOps&version=0.8.0"
#addin "Cake.Git&version=0.21.0"
#addin "Cake.Powershell&version=0.4.8"
#addin "Cake.FileHelpers&version=3.2.1"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

Setup<BuildParameters>(setupContext =>
{
    var buildParams = BuildParameters.GetParameters(setupContext);
    return buildParams;
});

//////////////////////////////////////////////////////////////////////
// CLEANUP
//////////////////////////////////////////////////////////////////////

Teardown<BuildParameters>((teardownContext, data) =>
{
    var sourceHash = data.Version.Sha;
    var customGitVersion = $"{data.Version.SemVer}+sha.{sourceHash.Substring(0,Math.Min(sourceHash.Length,7))}";
    Console.Out.WriteLine("##vso[build.updatebuildnumber]"+customGitVersion);

    DeleteDirectories(data.Paths.Directories.ToClean, new DeleteDirectorySettings{Recursive = true, Force = true});
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .WithCriteria<BuildParameters>((context, data) => data.IsLocalBuild)
    .Does<BuildParameters>(data =>
{
    CleanDirectories($"./src/**/obj/{data.Configuration}");
    CleanDirectories($"./src/**/bin/{data.Configuration}");
    CleanDirectories(data.Paths.Directories.Artifacts.FullPath);
});

Task("CleanAll")
    .WithCriteria<BuildParameters>((context, data) => data.IsLocalBuild)
    .Does<BuildParameters>(data =>
{
    CleanDirectories($"./src/**/obj");
    CleanDirectories($"./src/**/bin");
    CleanDirectories(data.Paths.Directories.Artifacts.FullPath);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does<BuildParameters>(data =>
{
	var settings = new DotNetCoreRestoreSettings();

    if(!data.IsLocalBuild)
    {
        // In VSTS though, we can use the SYSTEM_ACCESSTOKEN to restore from the nuget repos
        var systemAccessToken = data.SystemAccessToken ?? throw new Exception("Couldn't Find EnvironmentVariable 'SYSTEM_ACCESSTOKEN'");

        TransformTextFile(data.Paths.Files.NugetConfig,"{","}")
            .WithToken("API_KEY", systemAccessToken)
            .Save(data.Paths.Files.NugetConfig);

        settings.ConfigFile = data.Paths.Files.NugetConfig;
    }

    DotNetCoreRestore(data.Paths.Files.SlnFile.FullPath, settings);
});

Task("Db-Link-Server")
    .WithCriteria<BuildParameters>((context, data) => !data.IsLocalBuild)
    .Does(() => {
        //Remove $SERVER token when deploying db on same instance as CRM
        ReplaceTextInFiles("./src/Ocas.Domestic.Crm.Extras.Db.Ssdt/**/*.sql", "[$(SERVER)].", string.Empty);
        ReplaceTextInFiles("./src/Ocas.Domestic.Crm.Extras.Db.Ssdt/Ocas.Domestic.Crm.Extras.Db.Ssdt.sqlproj", "<ServerSqlCmdVariable>SERVER</ServerSqlCmdVariable>", string.Empty);
    });

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does<BuildParameters>(data =>
{
    Build(data);
});


Task("Deploy-Db")
    .IsDependentOn("Build")
    .Does<BuildParameters>(data =>
{
    var dbCliFile = GetFiles($"./src/**/{data.Configuration}/**/db.exe").First();
    var dacPacFile = GetFiles($"./src/**/{data.Configuration}/**/Ocas.Domestic.Crm.Extras.Db.Ssdt.dacpac").First();

    var arguments = new ProcessArgumentBuilder()
        .Append("deploy")
        .Append("-v")
        .Append($"-f \"{dacPacFile}\"")
        .Append("-i OCASINTSQL");

    if (data.IsPullRequest) {
        arguments = arguments
            .Append("-e CI")
            .Append("--connectionString=\"Data Source=ocasintsql.onco.local;Initial Catalog=OCAS_MSCRM.Extras_PR;User Id=cbui;Password=Ocas2014;\"");
    } else {
        arguments = arguments
            .Append("-e dev");
    }

    var exitCodeWithArgument = StartProcess(dbCliFile, new ProcessSettings{ Arguments = arguments });
    // This should output 0 as valid arguments supplied
    Information("Exit code: {0}", exitCodeWithArgument);

    if (exitCodeWithArgument != 0) {
        throw new ApplicationException("Deploy-Db failed to execute");
    }
});

Task("Create-Test-Lookups")
    .IsDependentOn("Db-Link-Server")
    .IsDependentOn("Deploy-Db")
    .Does<BuildParameters>(data =>
    {
        var resultCollection =StartPowershellFile("./lookup-generation/lookup-loader.ps1", new PowershellSettings()
            {
                OutputToAppConsole = false,
                FormatOutput = false,
                LogOutput = true
            }.WithArguments(args =>
            {
                if (data.IsPullRequest) {
                    args.Append("BinPath", $@".\src\Ocas.Domestic.Crm.Extras.Provider\bin\{data.Configuration}\net461\")
                            .AppendQuoted("ConnectionString", @"Data Source=ocasintsql.onco.local;Initial Catalog=OCAS_MSCRM.Extras_PR;User Id=cbui;Password=Ocas2014;");
                }
                else {
                    args.Append("BinPath", $@".\src\Ocas.Domestic.Crm.Extras.Provider\bin\{data.Configuration}\net461\")
                            .AppendQuoted("ConnectionString", @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=OCAS_MSCRM.Extras;Trusted_Connection=True;MultipleActiveResultSets=True;");
                }
            }));

        var returnCode = int.Parse(resultCollection[0].BaseObject.ToString());
        Information("Result: {0}", returnCode);

        if (returnCode != 0) {
            throw new ApplicationException("Script failed to execute");
        }

        Build(data);
    });

Task("Perform-Code-Analysis")
    .IsDependentOn("Verify-Code-Analysis")
    .IsDependentOn("Report-Code-Analysis-To-PullRequest");

Task("Verify-Code-Analysis")
    .IsDependentOn("Build")
    .Does<BuildParameters>(data =>
{
    var settings = new ReadIssuesSettings(data.Paths.Directories.Root);
    var issues = ReadIssues(
        new List<IIssueProvider>
        {
            MsBuildIssuesFromFilePath(
                data.Paths.Files.MsBuildFile,
                MsBuildXmlFileLoggerFormat)
        },
        settings);
    if(issues.Any()) throw new Exception($"Found '{issues.Count()}' issues during code analysis. Please review");
});

Task("Report-Code-Analysis-To-PullRequest")
    .WithCriteria<BuildParameters>((context, data) => data.IsPullRequest)
    .IsDependentOn("Build")
    .Does<BuildParameters>(data =>
{
    var pullRequestRepoUrl = EnvironmentVariable("BUILD_REPOSITORY_URI") ?? throw new ArgumentNullException("BUILD_REPOSITORY_URI");
    var pullRequestId = EnvironmentVariable("SYSTEM_PULLREQUEST_PULLREQUESTID") ?? throw new ArgumentNullException("SYSTEM_PULLREQUEST_PULLREQUESTID");

    //MSBuild
    ReportIssuesToPullRequest(
        MsBuildIssuesFromFilePath(data.Paths.Files.MsBuildFile, MsBuildXmlFileLoggerFormat),
        AzureDevOpsPullRequests(new Uri(pullRequestRepoUrl), Convert.ToInt32(pullRequestId), AzureDevOpsAuthenticationOAuth(data.SystemAccessToken)),
        data.Paths.Directories.Root);
});

Task("Unit-Tests")
    .WithCriteria<BuildParameters>((context, data) => data.IsPullRequest)
    .Does<BuildParameters>(data =>
{
    var projects = GetFiles(data.Paths.Directories.Solution + "/**/*UnitTests.csproj");

    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings{
            Configuration = data.Configuration,
            Logger = $"trx;LogFileName=TestResults.xml",
            NoBuild = true
        });
    }
});

Task("Integration-Tests")
    .Does<BuildParameters>(data =>
{
    var projects = GetFiles(data.Paths.Directories.Solution + "/**/*IntegrationTests.csproj");

    foreach(var project in projects)
    {
        DotNetCoreTest(project.FullPath, new DotNetCoreTestSettings{
            Configuration = data.Configuration,
            Logger = $"trx;LogFileName=TestResults.xml",
            NoBuild = true
        });
    }
});

Task("Copy-Db-Cli-To-Artifacts")
    .WithCriteria<BuildParameters>((context, data) => !data.IsPullRequest)
    .Does<BuildParameters>(data =>
{
    EnsureDirectoryExists(data.Paths.Directories.Artifacts);
    var solutionFolder = data.Paths.Directories.Artifacts.Combine($"{data.Paths.Files.SlnFile.GetFilenameWithoutExtension()}");

    CopyDirectory(data.Paths.Directories.Solution.Combine($"Ocas.Domestic.Crm.Extras.Db.Ssdt/bin/{data.Configuration}/"),  solutionFolder);
    CopyDirectory(data.Paths.Directories.Solution.Combine($"Ocas.Domestic.Crm.Extras.Db.Cli/bin/{data.Configuration}/net461/"), solutionFolder);
});

Task("Create-Crm-Entities")
    .WithCriteria<BuildParameters>((context, data) => data.IsLocalBuild)
    .Does<BuildParameters>(data =>
{
    var outFolder = data.Paths.Directories.Solution.Combine("Ocas.Domestic.Crm.Entities/");
    var crmSvcUtil = data.Paths.Directories.Root.CombineWithFilePath("./tools/DLaB.Xrm.EarlyBoundGenerator.Api.1.2019.5.17/content/bin/DLaB.EarlyBoundGenerator/CrmSvcUtil.exe");
    var sharedCrmSettings = new ProcessArgumentBuilder()
            .Append(@"/url:""http://devcrm.onco.local/OCAS/XRMServices/2011/Organization.svc""")
            .Append(@"/namespace:""Ocas.Domestic.Crm.Entities""")
            .Append(@"/username:""OCASAdminDev""")
            .Append(@"/password:""bF0jrExd""")
            .Append(@"/domain:""ONCO""");

    var outAction = outFolder.CombineWithFilePath("Actions.cs");
    var actionSettings= new ProcessArgumentBuilder()
            .Append(@"/generateActions")
            .Append(string.Format(@"/out:""{0}""", outAction.ToString()))
            .Append(@"/codecustomization:""DLaB.CrmSvcUtilExtensions.Action.CustomizeCodeDomService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/codegenerationservice:""DLaB.CrmSvcUtilExtensions.Action.CustomCodeGenerationService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/codewriterfilter:""DLaB.CrmSvcUtilExtensions.Action.CodeWriterFilterService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/metadataproviderservice:""DLaB.CrmSvcUtilExtensions.BaseMetadataProviderService,DLaB.CrmSvcUtilExtensions""");
    sharedCrmSettings.CopyTo(actionSettings);

    var generateActions = StartProcess(crmSvcUtil.ToString(), new ProcessSettings {Arguments = actionSettings });
    if(generateActions != 0) throw new Exception($"CRM Actions cannot be created with {actionSettings.Render()}.");

    var outOptionSet = outFolder.CombineWithFilePath("OptionSets.cs");
    var optionsSettings = new ProcessArgumentBuilder()
            .Append(string.Format(@"/out:""{0}""", outOptionSet.ToString()))
            .Append(@"/servicecontextname:""DomesticCrmServiceContext""")
            .Append(@"/codecustomization:""DLaB.CrmSvcUtilExtensions.OptionSet.CustomizeCodeDomService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/codegenerationservice:""DLaB.CrmSvcUtilExtensions.OptionSet.CustomCodeGenerationService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/codewriterfilter:""DLaB.CrmSvcUtilExtensions.OptionSet.CodeWriterFilterService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/namingservice:""DLaB.CrmSvcUtilExtensions.NamingService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/metadataproviderservice:""DLaB.CrmSvcUtilExtensions.BaseMetadataProviderService,DLaB.CrmSvcUtilExtensions""");
    sharedCrmSettings.CopyTo(optionsSettings);

    var generateOptions = StartProcess(crmSvcUtil.ToString(), new ProcessSettings {Arguments = optionsSettings });
    if(generateOptions != 0) throw new Exception($"CRM OptionSets cannot be created with {optionsSettings.Render()}.");

    var entitiesSettings = new ProcessArgumentBuilder()
            .Append(string.Format(@"/out:""{0}""", outFolder.ToString()))
            .Append(@"/servicecontextname:""DomesticCrmServiceContext""")
            .Append(@"/codecustomization:""DLaB.CrmSvcUtilExtensions.Entity.CustomizeCodeDomService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/codegenerationservice:""DLaB.CrmSvcUtilExtensions.Entity.CustomCodeGenerationService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/codewriterfilter:""DLaB.CrmSvcUtilExtensions.Entity.CodeWriterFilterService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/namingservice:""DLaB.CrmSvcUtilExtensions.NamingService,DLaB.CrmSvcUtilExtensions""")
            .Append(@"/metadataproviderservice:""DLaB.CrmSvcUtilExtensions.Entity.MetadataProviderService,DLaB.CrmSvcUtilExtensions""");
    sharedCrmSettings.CopyTo(entitiesSettings);

    var generateEntities = StartProcess(crmSvcUtil.ToString(), new ProcessSettings {Arguments = entitiesSettings });
    //Adds all entities files to the csproj and needs to be undone
    GitCheckout(data.Paths.Directories.Root,data.Paths.Directories.Solution.CombineWithFilePath("Ocas.Domestic.Crm.Entities/Ocas.Domestic.Crm.Entities.csproj"));
    if(generateEntities != 0) throw new Exception($"CRM Entities cannot be created with {entitiesSettings.Render()}.");
});

Task("Create-Crm-Dacpac")
    .WithCriteria<BuildParameters>((context, data) => data.IsLocalBuild)
    .Does<BuildParameters>(data =>
{
    var outFolder = data.Paths.Directories.Root.Combine("dacpacs");
    var sqlPackage = data.Paths.Directories.Root.CombineWithFilePath("./tools/SqlPackage.CommandLine.14.0.3953.4/tools/SqlPackage.exe");

    string database = "OCAS_MSCRM";
    string targetServer = "OCASINTSQL";
    string sourceUser = "cbui";
    string sourcePassword = "Ocas2014";
    string outDacpac = outFolder.CombineWithFilePath($"{database}.dacpac").ToString();

    var arguments = new ProcessArgumentBuilder()
            .Append(@"/Action:Extract")
            .Append($@"/SourceServerName:{targetServer}")
            .Append($@"/SourceDatabaseName:{database}")
            .Append($@"/TargetFile:{outDacpac}")
            .Append($@"/SourceUser:{sourceUser}")
            .Append($@"/SourcePassword:{sourcePassword}")
            .Append(@"/p:IgnoreUserLoginMappings=true");

    var generateDacpac = StartProcess(sqlPackage.ToString(), new ProcessSettings {Arguments = arguments });
    if(generateDacpac != 0) throw new Exception($"SqlPackage failed to be create dacpac with {arguments.Render()}.");
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Local")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build")
    .IsDependentOn("Create-Test-Lookups")
    .IsDependentOn("Unit-Tests")
    .IsDependentOn("Perform-Code-Analysis")
    .IsDependentOn("Copy-Db-Cli-To-Artifacts");

Task("Default")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Db-Link-Server")
    .IsDependentOn("Build")
    .IsDependentOn("Unit-Tests")
    .IsDependentOn("Perform-Code-Analysis")
    .IsDependentOn("Copy-Db-Cli-To-Artifacts");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

public void Build(BuildParameters data) {
     var settings = new MSBuildSettings()
        .WithTarget("Build")
            .SetConfiguration(data.Configuration)
            .SetMaxCpuCount(0)
            .WithProperty("Version", data.Version.SemVer)
            .WithProperty("PackageVersion", data.Version.SemVer)
            .WithProperty("FileVersion", data.Version.AssemblySemFileVer)
            .WithProperty("InformationalVersion", data.Version.InformationalVersion)
            .WithLogger(
                Context.Tools.Resolve("MSBuild.ExtensionPack.Loggers.dll").FullPath,
                "XmlFileLogger",
                string.Format("logfile=\"{0}\";verbosity=Detailed;encoding=UTF-8", data.Paths.Files.MsBuildFile)
            );

    if (!data.IsLocalBuild)
    {
        settings
            .WithWarningsAsMessage("SQL71562");
    }

	// Use MSBuild for the Sln, but deploy
	MSBuild(data.Paths.Files.SlnFile.FullPath, settings);
}