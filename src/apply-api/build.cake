//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#load "./build/parameters.cake"

#tool "nuget:?package=GitVersion.CommandLine&version=5.1.3"
#tool "nuget:?package=MSBuild.Extension.Pack&version=1.9.1"

#addin "Cake.Issues&version=0.8.1"
#addin "Cake.Issues.Reporting&version=0.8.0"
#addin "Cake.Issues.MsBuild&version=0.8.0"
#addin "Cake.AzureDevOps&version=0.4.4"
#addin "Cake.Issues.PullRequests&version=0.8.0"
#addin "Cake.Issues.PullRequests.AzureDevOps&version=0.8.0"

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
    var customGitVersion = $"{data.Version.AssemblySemVer}+sha.{sourceHash.Substring(0,Math.Min(sourceHash.Length,7))}";
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

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does<BuildParameters>(data =>
{
     var settings = new MSBuildSettings()
        .WithTarget("Build")
            .SetConfiguration(data.Configuration)
            .SetMaxCpuCount(0)
            .WithProperty("Version", data.Version.AssemblySemVer)
            .WithProperty("PackageVersion", data.Version.AssemblySemVer)
            .WithProperty("FileVersion", data.Version.AssemblySemFileVer)
            .WithProperty("InformationalVersion", data.Version.NuGetVersionV2)
            .WithLogger(
                Context.Tools.Resolve("MSBuild.ExtensionPack.Loggers.dll").FullPath,
                "XmlFileLogger",
                string.Format("logfile=\"{0}\";verbosity=Detailed;encoding=UTF-8", data.Paths.Files.MsBuildFile)
            );

	MSBuild(data.Paths.Files.SlnFile.FullPath, settings);
});

Task("Perform-Code-Analysis")
    .IsDependentOn("Report-Code-Analysis-To-PullRequest")
    .IsDependentOn("Verify-Code-Analysis");

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
    .IsDependentOn("Build")
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
    .IsDependentOn("Build")
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

Task("Package-Nuget")
    .WithCriteria<BuildParameters>((context, data) => !data.IsPullRequest)
    .IsDependentOn("Build")
    .Does<BuildParameters>(data =>
{
    EnsureDirectoryExists(data.Paths.Directories.Artifacts);

    var projects = GetFiles(data.Paths.Directories.Solution + "/**/Ocas.Domestic.Apply.Api.Client.csproj");

    var settings = new DotNetCorePackSettings {
        Configuration = data.Configuration,
        OutputDirectory = data.Paths.Directories.Artifacts,
        ArgumentCustomization = (args) => {
                return args
                    .Append("/p:Version={0}", data.Version.AssemblySemVer)
                    .Append("/p:AssemblyVersion={0}", data.Version.AssemblySemVer)
                    .Append("/p:FileVersion={0}", data.Version.AssemblySemFileVer)
                    .Append("/p:AssemblyInformationalVersion={0}", data.Version.NuGetVersionV2);
            }
    };

    projects.ToList().ForEach(project => DotNetCorePack(project.ToString(), settings));
});

Task("Publish-Api")
    .WithCriteria<BuildParameters>((context, data) => !data.IsPullRequest)
    .IsDependentOn("Build")
    .Does<BuildParameters>(data =>
{
    var settings = new DotNetCorePublishSettings
    {
        Configuration = data.Configuration,
        Framework = "net461",
        NoBuild = true
    };

    DotNetCorePublish(data.Paths.Files.SlnFile.FullPath, settings);
});

Task("Copy-Api-To-Artifacts")
    .WithCriteria<BuildParameters>((context, data) => !data.IsPullRequest)
    .IsDependentOn("Publish-Api")
    .Does<BuildParameters>(data =>
{
    EnsureDirectoryExists(data.Paths.Directories.Artifacts);

    var projectName = data.Paths.Files.SlnFile.GetFilenameWithoutExtension();
    Zip($"./src/{projectName}.Api/bin/{data.Configuration}/net461/publish/",data.Paths.Directories.Artifacts.CombineWithFilePath($"{projectName}.Api.zip"));
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Build")
    .IsDependentOn("Perform-Code-Analysis")
    .IsDependentOn("Unit-Tests")
    .IsDependentOn("Publish-Api")
    .IsDependentOn("Copy-Api-To-Artifacts");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);