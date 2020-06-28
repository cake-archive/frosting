using Cake.Common.Tools.GitReleaseManager;
using Cake.Common.Tools.GitReleaseManager.Create;
using Cake.Core;
using Cake.Frosting;

public class CreateReleaseNotes : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        if (string.IsNullOrEmpty(context.GitHubToken))
        {
            throw new CakeException("GitHub Token was not provided.");
        }

        var settings = new GitReleaseManagerCreateSettings
        {
            Milestone         = context.Version.Milestone,
            Name              = context.Version.Milestone,
            TargetCommitish   = context.PrimaryBranchName,
            Prerelease        = false
        };

        context.GitReleaseManagerCreate(context.GitHubToken, context.RepositoryOwner, context.RepositoryName, settings);
    }
}
