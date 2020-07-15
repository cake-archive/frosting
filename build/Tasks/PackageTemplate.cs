using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Common.Tools.DotNetCore.Pack;
using Cake.Common.Xml;
using Cake.Core.IO;
using Cake.Frosting;

public class PackageTemplate : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        if (context.AppVeyor)
        {
            context.XmlPoke(
                "./template/templates/cakefrosting/build/Build.csproj",
                "/Project/ItemGroup/PackageReference[@Include = 'Cake.Frosting']/@Version",
                context.Version.SemVersion
            );
        }

        var path = new FilePath("./template/Cake.Frosting.Template.csproj");
        context.DotNetCorePack(path.FullPath, new DotNetCorePackSettings
        {
            Configuration = context.BuildConfiguration,
            MSBuildSettings = new DotNetCoreMSBuildSettings()
                .WithProperty("Version", context.Version.SemVersion),
            OutputDirectory = context.Artifacts
        });
    }
}
