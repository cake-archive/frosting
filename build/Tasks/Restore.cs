using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Restore;
using Cake.Frosting;

[Dependency(typeof(Clean))]
public class Restore : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        context.DotNetCoreRestore("./src", new DotNetCoreRestoreSettings
        {
            MSBuildSettings = context.MSBuildSettings,
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