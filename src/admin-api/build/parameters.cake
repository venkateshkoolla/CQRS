#load "./paths.cake"

public class BuildParameters
{
    public string Configuration { get; private set; }
    public bool IsLocalBuild { get; private set; }
    public bool IsRunningOnUnix { get; private set; }
    public bool IsRunningOnWindows { get; private set; }
    public bool IsRunningOnAzureDevOps { get; private set; }
    public bool IsRunningOnPremTfsAgent { get; private set; }
    public bool IsPullRequest { get; private set; }
    public bool IsMainMassTransitRepo { get; private set; }
    public bool IsMasterBranch { get; private set; }
    public bool IsDevelopBranch { get; private set; }
    public bool IsTagged { get; private set; }
    public string SystemAccessToken { get; private set; }
    public BuildPaths Paths { get; private set; }
    public GitVersion Version { get; private set; }

    public static BuildParameters GetParameters(ICakeContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }

        var target = context.Argument("target", "Default");
        var buildSystem = context.BuildSystem();

        return new BuildParameters {
            Configuration = context.Argument("configuration", "Release"),
            IsLocalBuild = buildSystem.IsLocalBuild,
            IsRunningOnUnix = context.IsRunningOnUnix(),
            IsRunningOnWindows = context.IsRunningOnWindows(),
            IsRunningOnAzureDevOps = buildSystem.TFBuild.IsRunningOnAzurePipelinesHosted,
            IsRunningOnPremTfsAgent = buildSystem.TFBuild.IsRunningOnAzurePipelines,
            IsPullRequest = buildSystem.IsPullRequest,
            IsMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildSystem.TFBuild.Environment.Repository.SourceBranchName),
            IsDevelopBranch = StringComparer.OrdinalIgnoreCase.Equals("develop", buildSystem.TFBuild.Environment.Repository.SourceBranchName),
            IsTagged = IsBuildTagged(buildSystem),
            SystemAccessToken = context.EnvironmentVariable("SYSTEM_ACCESSTOKEN"),
            Paths = BuildPaths.GetPaths(context),
            Version = context.GitVersion(new GitVersionSettings{UpdateAssemblyInfo = !buildSystem.IsLocalBuild}) // this only updates assemblyinfo.cs files, not the new csproj, we pass in the arguments for the new csproj
        };
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.TFBuild.Environment.Repository.SourceBranchName.StartsWith("refs/tags");
    }
}
