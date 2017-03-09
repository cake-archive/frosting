using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Pack;
using Cake.Frosting;

public class PackageTemplate : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        context.NuGetPack("./template/Cake.Frosting.Template.nuspec", new NuGetPackSettings
        {
            Version = context.Version.GetSemanticVersion(),
            OutputDirectory = context.Artifacts,
            NoPackageAnalysis = true
        });
    }
}