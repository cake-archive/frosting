using System;
using System.Linq;
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
        context.BuildConfiguration = context.Argument<string>("configuration", "Release");
        context.ForcePublish = context.Argument<bool>("forcepublish", false);
        context.AzureArtifactsSourceUrl = GetEnvironmentValueOrArgument(context, "FROSTING_AZURE_ARTIFACTS_SOURCE_URL", "azureartifactssourceurl");
        context.AzureArtifactsPersonalAccessToken = GetEnvironmentValueOrArgument(context, "FROSTING_AZURE_ARTIFACTS_PERSONAL_ACCESS_TOKEN", "mygetapikey");
        context.AzureArtifactsSourceName = GetEnvironmentValueOrArgument(context, "FROSTING_AZURE_ARTIFACTS_SOURCE_NAME", "azureartifactssourcename");
        context.AzureArtifactsSourceUserName = GetEnvironmentValueOrArgument(context, "FROSTING_AZURE_ARTIFACTS_SOURCE_USER_NAME", "azureartifactssourceusername");
        context.GitHubToken = GetEnvironmentValueOrArgument(context, "FROSTING_GITHUB_TOKEN", "githubtoken");

        // Directories
        context.Artifacts = new DirectoryPath("./artifacts");

        // Build system information.
        var buildSystem = context.BuildSystem();
        context.AppVeyor = buildSystem.AppVeyor.IsRunningOnAppVeyor;
        context.IsLocalBuild = buildSystem.IsLocalBuild;
        context.IsPullRequest = buildSystem.AppVeyor.Environment.PullRequest.IsPullRequest;
        context.PrimaryBranchName = "master";
        context.RepositoryOwner = "cake-build";
        context.RepositoryName = "frosting";
        context.IsOriginalRepo = StringComparer.OrdinalIgnoreCase.Equals(string.Concat(context.RepositoryOwner, "/", context.RepositoryName), buildSystem.AppVeyor.Environment.Repository.Name);
        context.IsTagged = IsBuildTagged(buildSystem);
        context.IsPrimaryBranch = StringComparer.OrdinalIgnoreCase.Equals(context.PrimaryBranchName, buildSystem.AppVeyor.Environment.Repository.Branch);
        context.BuildSystem = buildSystem;

        // Install tools
        context.Information("Installing tools...");
        context.InstallNuGetExe("5.6.0");

        // Install Global .Net Tools
        context.Information("Installing .Net Global Tools...");
        context.DotNetCoreToolInstall("GitReleaseManager.Tool", "0.11.0", "dotnet-gitreleasemanager");
        context.DotNetCoreToolInstall("SignClient", "1.2.109", "SignClient");
        context.DotNetCoreToolInstall("GitVersion.Tool", "5.1.2", "dotnet-gitversion");

        // Calculate semantic version.
        context.Version = BuildVersion.Calculate(context);
        context.Version.Version = context.Argument<string>("version", context.Version.Version);
        context.Version.SemVersion = context.Argument<string>("suffix", context.Version.SemVersion);

        // MSBuild Settings
        context.MSBuildSettings =  new DotNetCoreMSBuildSettings()
                            .WithProperty("Version", context.Version.SemVersion)
                            .WithProperty("AssemblyVersion", context.Version.Version)
                            .WithProperty("FileVersion", context.Version.Version);

        if (context.AppVeyor)
        {
            context.MSBuildSettings.WithProperty("ContinuousIntegrationBuild", "true");
        }

        if(!context.IsRunningOnWindows())
        {
        var frameworkPathOverride = context.Environment.Runtime.IsCoreClr
                                        ?   new []{
                                                new DirectoryPath("/Library/Frameworks/Mono.framework/Versions/Current/lib/mono"),
                                                new DirectoryPath("/usr/lib/mono"),
                                                new DirectoryPath("/usr/local/lib/mono")
                                            }
                                            .Select(directory =>directory.Combine("4.5"))
                                            .FirstOrDefault(directory => context.FileSystem.Exist(directory))
                                            ?.FullPath + "/"
                                        : new FilePath(typeof(object).Assembly.Location).GetDirectory().FullPath + "/";

            // Use FrameworkPathOverride when not running on Windows.
            context.Information("Build will use FrameworkPathOverride={0} since not building on Windows.", frameworkPathOverride);
            context.MSBuildSettings.WithProperty("FrameworkPathOverride", frameworkPathOverride);
        }

        context.Information("Version: {0}", context.Version);
        context.Information("Sem version: {0}", context.Version.SemVersion);
        context.Information("Configuration: {0}", context.BuildConfiguration);
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
