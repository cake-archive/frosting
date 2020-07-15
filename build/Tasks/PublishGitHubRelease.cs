using Cake.Common.Tools.GitReleaseManager;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

[Dependency(typeof(Package))]
[Dependency(typeof(PublishNuGet))]
public class PublishGitHubRelease : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return !context.IsLocalBuild &&
            !context.IsPullRequest &&
            context.IsOriginalRepo &&
            context.IsPrimaryBranch &&
            context.BuildSystem.IsRunningOnAppVeyor &&
            context.Environment.Platform.Family == PlatformFamily.Windows &&
            context.IsTagged;
    }

    public override void Run(Context context)
    {
        if (string.IsNullOrEmpty(context.GitHubToken))
        {
            throw new CakeException("GitHub Token was not provided.");
        }

        // Get the file paths.
        var files = new[] {
            $"./artifacts/Cake.Frosting.Template.{context.Version.SemVersion}.nupkg",
            $"./artifacts/Cake.Frosting.{context.Version.SemVersion}.nupkg"
        };

        // Concatenating FilePathCollections should make sure we get unique FilePaths
        foreach (var file in files)
        {
            context.GitReleaseManagerAddAssets(context.GitHubToken, context.RepositoryOwner, context.RepositoryName, context.Version.Milestone, file);
        }

        context.GitReleaseManagerClose(context.GitHubToken, context.RepositoryOwner, context.RepositoryName, context.Version.Milestone);
    }
}
