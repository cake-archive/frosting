using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Core;
using Cake.Frosting;

[Dependency(typeof(Restore))]
public class Build : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        context.DotNetCoreBuild("./src/Cake.Frosting.sln", new DotNetCoreBuildSettings {
            Configuration = context.Configuration,
            NoRestore = true,
            MSBuildSettings = context.MSBuildSettings
        });
    }
}