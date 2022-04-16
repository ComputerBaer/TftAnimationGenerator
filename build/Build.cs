using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter]
    readonly string Version = "0.0.0";

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;

    static readonly TargetRuntime[] TargetRuntimes =
    {
        new("win-x64", "Win64", "zip"),
        new("win-x86", "Win32", "zip"),
        new("linux-x64", "Linux64", "tgz"),
    };

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath TestsDirectory => RootDirectory / "tests";
    AbsolutePath OutputDirectory => RootDirectory / "output";

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(OutputDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoRestore());
        });

    Target Publish => _ => _
        .DependsOn(Clean, Compile)
        .Executes(() =>
        {
            foreach (var runtime in TargetRuntimes)
            {
                DotNetPublish(s => s
                    .SetConfiguration(Configuration)
                    .SetVersion(Version)
                    .SetRuntime(runtime.BuildRuntime)
                    .EnablePublishSingleFile()
                    .DisableSelfContained()
                    .SetOutput(OutputDirectory / runtime.BuildRuntime)
                );
                
                CompressionTasks.Compress(OutputDirectory / runtime.BuildRuntime, OutputDirectory / $"TftAnimationGenerator_{runtime.FileName}.{runtime.FileExt}", info => !info.Name.EndsWith(".pdb"));
            }
        });

    private record TargetRuntime(string BuildRuntime, string FileName, string FileExt);
}
