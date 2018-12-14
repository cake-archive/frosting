using Cake.Common.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

public class Context : FrostingContext
{
    public string Target { get; set; }
    public string Configuration { get; set; }
    public BuildVersion Version { get; set; }
    public DotNetCoreMSBuildSettings MSBuildSettings { get; set; }

    public DirectoryPath Artifacts { get; set; }

    public string MyGetSource { get; set; }
    public string MyGetApiKey { get; set; }
    
    public bool IsLocalBuild { get; set; }
    public bool IsPullRequest { get; set; }
    public bool IsOriginalRepo { get; set; }
    public bool IsTagged { get; set; }
    public bool IsMasterBranch { get; set; }
    public bool ForcePublish { get; set; }

    public bool AppVeyor { get; set; }

    public BuildSystem BuildSystem { get; set; }

    public Context(ICakeContext context)
        : base(context)
    {
    }
}