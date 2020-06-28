using System;
using Cake.Common;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Frosting;

[Dependency(typeof(Package))]
[Dependency(typeof(PublishAzureArtifacts))]
public class PublishNuGet : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return !context.IsLocalBuild &&
            !context.IsPullRequest &&
            context.IsOriginalRepo &&
            context.IsPrimaryBranch &&
            context.IsTagged;
    }

    public override void Run(Context context)
    {
        // Resolve the API key.
        var apiKey = context.EnvironmentVariable("NUGET_API_KEY");
        if(string.IsNullOrEmpty(apiKey)) {
            throw new InvalidOperationException("Could not resolve NuGet API key.");
        }

        // Get the packages...
        var packages = new[] {
            $"./artifacts/Cake.Frosting.Template.{context.Version.SemVersion}.nupkg",
            $"./artifacts/Cake.Frosting.{context.Version.SemVersion}.nupkg",
        };

        // Resolve the API url.
        var apiUrl = context.EnvironmentVariable("NUGET_API_URL");
        if(string.IsNullOrEmpty(apiUrl)) {
            throw new InvalidOperationException("Could not resolve NuGet API url.");
        }

        foreach(var package in packages)
        {
            // Push the package.
            context.NuGetPush(package, new NuGetPushSettings {
                ApiKey = apiKey,
                Source = apiUrl
            });
        }
    }
}
