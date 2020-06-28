using System;
using System.Linq;
using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Frosting;

[Dependency(typeof(Package))]
public class SignFiles : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return (!context.IsLocalBuild &&
                !context.IsPullRequest &&
                context.IsOriginalRepo &&
                context.IsPrimaryBranch &&
                context.IsTagged) ||
                StringComparer.OrdinalIgnoreCase.Equals(context.EnvironmentVariable("SIGNING_TEST"), "True");
    }

    public override void Run(Context context)
    {
        var signClientPath = context.Tools.Resolve("SignClient.exe") ?? context.Tools.Resolve("SignClient") ?? throw new Exception("Failed to locate sign tool");

        // Get the secret.
        var secret = context.EnvironmentVariable("SIGNING_SECRET");
        if(string.IsNullOrWhiteSpace(secret)) {
            throw new InvalidOperationException("Could not resolve signing secret.");
        }
        // Get the user.
        var user = context.EnvironmentVariable("SIGNING_USER");
        if(string.IsNullOrWhiteSpace(user)) {
            throw new InvalidOperationException("Could not resolve signing user.");
        }

        var settings = new FilePath("./signclient.json");
        var filter = new FilePath("./signclient.filter");

        // Get the files to sign.
        var files = new[] {
            $"./artifacts/Cake.Frosting.Template.{context.Version.SemVersion}.nupkg",
            $"./artifacts/Cake.Frosting.{context.Version.SemVersion}.nupkg",
            $"./artifacts/Cake.Frosting.{context.Version.SemVersion}.snupkg"
        };

        foreach(var file in files)
        {
            context.Information("Signing {0}...", file);

            // Build the argument list.
            var arguments = new ProcessArgumentBuilder()
                .Append("sign")
                .AppendSwitchQuoted("-c", context.MakeAbsolute(settings).FullPath)
                .AppendSwitchQuoted("-i", context.MakeAbsolute(((FilePath)file)).FullPath)
                .AppendSwitchQuoted("-f", context.MakeAbsolute(filter).FullPath)
                .AppendSwitchQuotedSecret("-s", secret)
                .AppendSwitchQuotedSecret("-r", user)
                .AppendSwitchQuoted("-n", "Cake")
                .AppendSwitchQuoted("-d", "Cake (C# Make) is a cross platform build automation system.")
                .AppendSwitchQuoted("-u", "https://cakebuild.net");

            // Sign the binary.
            var result = context.StartProcess(signClientPath.FullPath, new ProcessSettings {  Arguments = arguments });
            if(result != 0)
            {
                // We should not recover from this.
                throw new InvalidOperationException("Signing failed!");
            }
        }
    }
}
