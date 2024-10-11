public class BuildPaths
{
    public BuildDirectories Directories { get; private set; }
    public BuildFiles Files { get; private set; }

    public static BuildPaths GetPaths(
        ICakeContext context
        )
    {
        if (context == null)
        {
            throw new ArgumentNullException("context");
        }


        // Directories
        var rootDir = context.MakeAbsolute(context.Directory("./"));
        var artifactsDir = context.EnvironmentVariable("BUILD_ARTIFACTSTAGINGDIRECTORY") ?? context.MakeAbsolute(rootDir.Combine("artifacts")).FullPath;
        var solutionDir = rootDir.Combine("src");

        var buildDirectories = new BuildDirectories(
            artifactsDir,
            rootDir,
            solutionDir
        );

        // Files
        var msBuildFile = rootDir.CombineWithFilePath("msbuild.log");
        var slnFile = solutionDir.CombineWithFilePath("Ocas.Domestic.Data.sln");
        var nugetConfig = context.MakeAbsolute(rootDir.CombineWithFilePath("../../nuget-src.config"));

        var buildFiles = new BuildFiles(
            msBuildFile,
            slnFile,
            nugetConfig
        );

        return new BuildPaths
        {
            Directories = buildDirectories,
            Files = buildFiles
        };
    }
}

public class BuildFiles
{
    public FilePath MsBuildFile { get; private set; }
    public FilePath SlnFile { get; private set; }
    public FilePath NugetConfig { get; private set; }

    public BuildFiles(
        FilePath msBuildFile,
        FilePath slnFile,
        FilePath nugetConfig
        )
    {
        MsBuildFile = msBuildFile;
        NugetConfig = nugetConfig;
        SlnFile = slnFile;
    }
}

public class BuildDirectories
{
    public DirectoryPath Artifacts { get; }
    public DirectoryPath Root { get; }
    public DirectoryPath Solution { get; }
    public ICollection<DirectoryPath> ToClean { get; }

    public BuildDirectories(
        DirectoryPath artifactsDir,
        DirectoryPath rootDir,
        DirectoryPath solutionDir
        )
    {
        Artifacts = artifactsDir;
        Root = rootDir;
        Solution = solutionDir;
        ToClean = new List<DirectoryPath>();
    }
}
