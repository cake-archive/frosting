using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Common.Tools.NuGet.Sources;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

[Dependency(typeof(Package))]
[Dependency(typeof(AppVeyorArtifacts))]
public class PublishAzureArtifacts : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return !context.IsLocalBuild && !context.IsPullRequest && context.IsOriginalRepo
            && (context.IsTagged || !context.IsPrimaryBranch);
    }

    public override void Run(Context context)
    {
        if(context.AzureArtifactsSourceUrl == null)
        {
            throw new CakeException("Azure Artifacts source URL was not provided.");
        }
        if(context.AzureArtifactsSourceName == null)
        {
            throw new CakeException("Azure Artifacts source name was not provided.");
        }
        if(context.AzureArtifactsPersonalAccessToken == null)
        {
            throw new CakeException("Azure Artifacts Personal Access Token was not provided.");
        }
        if(context.AzureArtifactsSourceUserName == null)
        {
            throw new CakeException("Azure Artifacts username was not provided.");
        }

        // Get the file paths.
        var root = new DirectoryPath("./src/Cake.Frosting");
        var files = new[] {
            $"./artifacts/Cake.Frosting.Template.{context.Version.SemVersion}.nupkg",
            $"./artifacts/Cake.Frosting.{context.Version.SemVersion}.nupkg"
        };

        if(!context.NuGetHasSource(context.AzureArtifactsSourceName))
        {
            context.NuGetAddSource(context.AzureArtifactsSourceName, context.AzureArtifactsSourceUrl, new NuGetSourcesSettings{
                UserName = context.AzureArtifactsSourceUserName,
                Password = context.AzureArtifactsPersonalAccessToken
            });
        }

        // Push files
        foreach(var file in files)
        {
            context.NuGetPush(file, new NuGetPushSettings {
                Source = context.AzureArtifactsSourceName,
                ApiKey = "az"
            });
        }
    }
}
