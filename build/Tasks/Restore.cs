using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Frosting;

[Dependency(typeof(UpdateVersionInfo))]
public class Restore : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetCoreRestore("./src", new DotNetCoreRestoreSettings
        {
            Verbose = false,
            Verbosity = DotNetCoreRestoreVerbosity.Warning,
            Sources = new [] {
                "https://www.myget.org/F/xunit/api/v3/index.json",
                "https://dotnet.myget.org/F/dotnet-core/api/v3/index.json",
                "https://dotnet.myget.org/F/cli-deps/api/v3/index.json",
                "https://api.nuget.org/v3/index.json",
                "https://www.myget.org/F/cake/api/v3/index.json"
            }
        });
    }
}