#load "version.cake"
#load "utilities.cake"

public class Parameters
{
    public string Target { get; set; }
    public string Configuration { get; set; }
    public string Version { get; set; }
    public string Suffix { get; set; }
    public bool IsRunningOnAppVeyor { get; private set; }
    public Project[] Projects { get; set; }

    public string MyGetSource { get; set; }
    public string MyGetApiKey { get; set; }

    public bool IsLocalBuild { get; set; }
    public bool IsPullRequest { get; set; }
    public bool IsOriginalRepo { get; set; }
    public bool IsTagged { get; set; }
    public bool IsMasterBranch { get; set; }

    public bool ForcePublish { get; set; }

    public bool ShouldPublishToMyGet
    {
        get
        {
            return ForcePublish || (!IsLocalBuild && !IsPullRequest && IsOriginalRepo 
                && (IsTagged || !IsMasterBranch));
        }
    }

    public static Parameters Create(ICakeContext context)
    {
        var parameters = new Parameters();

        parameters.Target = context.Argument<string>("target", "Default");
        parameters.Configuration = context.Argument<string>("configuration", "Release");
        parameters.Version = context.Argument<string>("version", null);
        parameters.Suffix = context.Argument<string>("suffix", null);
        parameters.MyGetSource = GetEnvironmentValueOrArgument(context, "FROSTING_MYGET_SOURCE", "mygetsource");
        parameters.MyGetApiKey = GetEnvironmentValueOrArgument(context, "FROSTING_MYGET_API_KEY", "mygetapikey");
        parameters.IsRunningOnAppVeyor = context.BuildSystem().AppVeyor.IsRunningOnAppVeyor;

        var buildSystem = context.BuildSystem();
        parameters.IsLocalBuild = buildSystem.IsLocalBuild;
        parameters.IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        parameters.IsOriginalRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/frosting", buildSystem.AppVeyor.Environment.Repository.Name);
        parameters.IsTagged = IsBuildTagged(buildSystem);
        parameters.IsMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildSystem.AppVeyor.Environment.Repository.Branch);

        parameters.ForcePublish = context.Argument<bool>("forcepublish", false);

        parameters.Projects = new Project[] {
            new Project { Name = "Cake.Frosting", Path = "./src/Cake.Frosting/project.json", Publish = true },
            new Project { Name = "Cake.Frosting.Tests" ,Path = "./src/Cake.Frosting.Tests/project.json" },
            new Project { Name = "Sandbox", Path = "./src/Sandbox/project.json" },
            new Project { Name = "Cake.Frosting.Cli", Path = "./src/Cake.Frosting.Cli/project.json", Publish = true }
        };

        return parameters;
    }

    public void Setup(ICakeContext context)
    {
        var info = BuildVersion.GetVersion(context, this);
        Version = Version ?? info.Version;
        Suffix = Suffix ?? info.Suffix;
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }
}