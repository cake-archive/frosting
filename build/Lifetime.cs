using System;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.Build;
using Cake.Frosting;
using Cake.Core.Diagnostics;

public class Lifetime : FrostingLifetime<Context>
{
    public override void Setup(Context context)
    {
        context.Target = context.Argument<string>("target", "Default");
        context.Configuration = context.Argument<string>("configuration", "Release");
        context.ForcePublish = context.Argument<bool>("forcepublish", false);
        context.MyGetSource = GetEnvironmentValueOrArgument(context, "FROSTING_MYGET_SOURCE", "mygetsource");
        context.MyGetApiKey = GetEnvironmentValueOrArgument(context, "FROSTING_MYGET_API_KEY", "mygetapikey");

        // Build system information.
        var buildSystem = context.BuildSystem();
        context.AppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        context.IsLocalBuild = buildSystem.IsLocalBuild;
        context.IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        context.IsOriginalRepo = StringComparer.OrdinalIgnoreCase.Equals("cake-build/frosting", buildSystem.AppVeyor.Environment.Repository.Name);
        context.IsTagged = IsBuildTagged(buildSystem);
        context.IsMasterBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildSystem.AppVeyor.Environment.Repository.Branch);

        // Install tools
        context.Information("Installing tools...");
        ToolInstaller.Install(context, "GitVersion.CommandLine", "3.6.2");

        // Calculate semantic version.
        var version = BuildVersion.Calculate(context);
        context.Version = context.Argument<string>("version", version.Version);
        context.Suffix = context.Argument<string>("suffix", version.Suffix);

        context.Information("Version: {0}", context.Version);
        context.Information("Version suffix: {0}", context.Suffix);
        context.Information("Configuration: {0}", context.Configuration);
        context.Information("Target: {0}", context.Target);
        context.Information("AppVeyor: {0}", context.AppVeyor);
    }

    private static bool IsBuildTagged(BuildSystem buildSystem)
    {
        return buildSystem.AppVeyor.Environment.Repository.Tag.IsTag
            && !string.IsNullOrWhiteSpace(buildSystem.AppVeyor.Environment.Repository.Tag.Name);
    }

    private static string GetEnvironmentValueOrArgument(Context context, string environmentVariable, string argumentName)
    {
        var arg = context.EnvironmentVariable(environmentVariable);
        if (string.IsNullOrWhiteSpace(arg))
        {
            arg = context.Argument<string>(argumentName, null);
        }
        return arg;
    }
}