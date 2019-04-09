using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

[Dependency(typeof(UnitTests))]
[Dependency(typeof(PackageTemplate))]
public class Package : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        var path = new FilePath("./src/Cake.Frosting/Cake.Frosting.csproj");
        context.DotNetCorePack(path.FullPath, new DotNetCorePackSettings {
            Configuration = context.BuildConfiguration,
            MSBuildSettings = context.MSBuildSettings,
            NoBuild = true,
            NoRestore = true,
            OutputDirectory = context.Artifacts,
            ArgumentCustomization = args => args.Append("--include-symbols --include-source")
        });
    }
}