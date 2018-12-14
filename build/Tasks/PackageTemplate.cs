using System.Collections.Generic;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Common.Xml;
using Cake.Frosting;

public class PackageTemplate : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        if (context.AppVeyor)
        {
            context.XmlPoke(
                "./template/Build.csproj",
                "/Project/ItemGroup/PackageReference[@Include = 'Cake.Frosting']/@Version",
                context.Version.SemVersion
            );
        }

        context.NuGetPack("./template/Cake.Frosting.Template.nuspec", new NuGetPackSettings
        {
            Version = context.Version.SemVersion,
            OutputDirectory = context.Artifacts,
            NoPackageAnalysis = true
        });
    }
}