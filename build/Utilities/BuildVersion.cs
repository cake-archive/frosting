using Cake.Common;
using Cake.Common.Diagnostics;
using Cake.Common.Tools.GitVersion;
using Cake.Core;

public class BuildVersion
{
    public string Version { get; set; }
    public string SemVersion { get; set; }
    public string Milestone { get; private set; }
    public string InformationalVersion { get; private set; }
    public string FullSemVersion { get; private set; }
    public string AssemblySemVer { get; private set; }

    public BuildVersion(string version, string semVersion, string informationalVersion, string assemblySemVer, string milestone, string fullSemVersion)
    {
        Version = version;
        SemVersion = semVersion;
        InformationalVersion = informationalVersion;
        AssemblySemVer = assemblySemVer;
        Milestone = milestone;
        FullSemVersion = fullSemVersion;
    }

    public static BuildVersion Calculate(Context context)
    {
        string version = null;
        string semVersion = null;
        string milestone = null;
        string informationalVersion = null;
        string assemblySemVer = null;
        string fullSemVersion = null;

        if (context.IsRunningOnWindows())
        {
            context.Information("Calculating semantic version...");
            if (!context.IsLocalBuild)
            {
                context.GitVersion(new GitVersionSettings
                {
                    OutputType = GitVersionOutput.BuildServer
                });
            }

            GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
            {
                OutputType = GitVersionOutput.Json
            });
            version = assertedVersions.MajorMinorPatch;
            semVersion = assertedVersions.LegacySemVerPadded;
            informationalVersion = assertedVersions.InformationalVersion;
            assemblySemVer = assertedVersions.AssemblySemVer;
            milestone = string.Concat("v", version);     
            fullSemVersion = assertedVersions.FullSemVer;
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new CakeException("Could not calculate version of build.");
        }

        return new BuildVersion(version, semVersion, informationalVersion, assemblySemVer, milestone, fullSemVersion);
    }
}