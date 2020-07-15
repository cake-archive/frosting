using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

[Dependency(typeof(Package))]
[Dependency(typeof(SignFiles))]
public class AppVeyorArtifacts : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return context.AppVeyor;
    }

    public override void Run(Context context)
    {
        // Get the file paths.
        var files = new[] {
            $"./artifacts/Cake.Frosting.Template.{context.Version.SemVersion}.nupkg",
            $"./artifacts/Cake.Frosting.{context.Version.SemVersion}.nupkg"
        };


        // Push files
        foreach(var file in files)
        {
            context.BuildSystem.AppVeyor.UploadArtifact(
                file
            );
        }
    }
}
