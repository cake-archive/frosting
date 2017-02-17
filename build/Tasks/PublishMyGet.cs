using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

[Dependency(typeof(Package))]
public class PublishMyGet : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return !context.IsLocalBuild && !context.IsPullRequest && context.IsOriginalRepo 
            && (context.IsTagged || !context.IsMasterBranch);
    }

    public override void Run(Context context)
    {
        if(context.MyGetSource == null)
        {
            throw new CakeException("MyGet source was not provided.");
        }
        if(context.MyGetApiKey == null)
        {
            throw new CakeException("MyGet API key was not provided.");
        }

        // Get the file paths.
        var root = new DirectoryPath("./src/Cake.Frosting");
        var packageVersion = string.Concat(context.Version, "-", context.Suffix).Trim('-');
        var files = new[] {
            $"{root.FullPath}/bin/{context.Configuration}/Cake.Frosting.{packageVersion}.nupkg",
            $"{root.FullPath}/bin/{context.Configuration}/Cake.Frosting.{packageVersion}.symbols.nupkg"
        };

        // Push files
        foreach(var file in files) 
        {
            context.NuGetPush(file, new NuGetPushSettings() {
                Source = context.MyGetSource,
                ApiKey = context.MyGetApiKey
            });
        }
    }
}