using System;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Frosting;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

public class Lifetime : FrostingLifetime<Context>
{
    public override void Setup(Context context)
    {
        // Arguments
        context.Target = context.Argument<string>("target", "Default");
        context.Configuration = context.Argument<string>("configuration", "Release");
        context.ForcePublish = context.Argument<bool>("forcepublish", false);
        context.MyGetSource = GetEnvironmentValueOrArgument(context, "FROSTING_MYGET_SOURCE", "mygetsource");
        context.MyGetApiKey = GetEnvironmentValueOrArgument(context, "FROSTING_MYGET_API_KEY", "mygetapikey");

        // Directories
        context.Artifacts = new DirectoryPath("./artifacts");

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
        context.Version = BuildVersion.Calculate(context);
        context.Version.Version = context.Argument<string>("version", context.Version.Version);
        context.Version.SemVersion = context.Argument<string>("suffix", context.Version.SemVersion);

        // MSBuild Settings
        context.MSBuildSettings =  new DotNetCoreMSBuildSettings()
                            .WithProperty("Version", context.Version.SemVersion)
                            .WithProperty("AssemblyVersion", context.Version.Version)
                            .WithProperty("FileVersion", context.Version.Version);

        context.Information("Version: {0}", context.Version);
        context.Information("Sem version: {0}", context.Version.SemVersion);
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