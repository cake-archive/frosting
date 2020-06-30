using System;
using System.IO;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Common.Net;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Install;
using Cake.Core;
using Cake.Core.IO;

public static class ToolInstaller
{
    private static DirectoryPath ToolsPath { get; } = "./tools";

    public static void InstallNuGetExe(this ICakeContext context, string version)
    {

        var toolsPath = context.MakeAbsolute(ToolsPath);

        context.EnsureDirectoryExists(toolsPath);

        var nugetExePath = toolsPath.CombineWithFilePath("nuget.exe");

        if (context.FileExists(nugetExePath))
        {
            if (TryValidateVersion(version, nugetExePath, out var existingVersion))
            {
                context.Tools.RegisterFile(nugetExePath);
                return;
            }

            context.Information(
                "Found version {0} expected {1}, deleting {2}...",
                existingVersion,
                version,
                toolsPath
            );

            context.DeleteFile(nugetExePath);
        }

        var address = $"https://dist.nuget.org/win-x86-commandline/v{Uri.EscapeDataString(version)}/nuget.exe";
        context.Information("Downloading NuGet exe from {0} to {1}...", address, nugetExePath);
        context.DownloadFile(
            address,
            nugetExePath
        );

        if (!context.FileExists(nugetExePath))
        {
            throw new Exception("Failed to install NuGet exe.");
        }


        if (!TryValidateVersion(version, nugetExePath, out var downloadedFileVersion))
        {
            throw new Exception($"Expected version {version} got {downloadedFileVersion}");
        }

        context.Tools.RegisterFile(nugetExePath);
        context.Information("NuGet exe downloaded and registered successfully.");
    }

    private static bool TryValidateVersion(string expectedVersion, FilePath nugetExePath, out string foundVersion)
    {
        try
        {
            var fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(nugetExePath.FullPath);
            foundVersion = FormattableString.Invariant($"{fileVersion.FileMajorPart}.{fileVersion.FileMinorPart}.{fileVersion.FileBuildPart}");
            return foundVersion == expectedVersion;
        }
        catch(Exception ex)
        {
            foundVersion = ex.Message;
            return false;
        }
    }

    public static void Install(this ICakeContext context, string package, string version)
    {
        context.NuGetInstall(package, new NuGetInstallSettings {
            Version = version,
            ExcludeVersion = true,
            OutputDirectory = ToolsPath
        });
    }

    public static FilePath DotNetCoreToolInstall(
        this ICakeContext context,
        string package,
        string version,
        string toolName)
    {
        context.EnsureDirectoryExists(ToolsPath);

        var toolsPath = context.MakeAbsolute(ToolsPath);

        var toolInstallPath = toolsPath
                                .Combine(".store")
                                .Combine(package.ToLowerInvariant())
                                .Combine(version.ToLowerInvariant());

        var toolPath = toolsPath.CombineWithFilePath(
                        string.Concat(
                            toolName,
                            context.Environment.Platform.IsUnix()
                                ? string.Empty
                                : ".exe"
                            )
                        );

        if (!context.DirectoryExists(toolInstallPath) && context.FileExists(toolPath))
        {
            context.DotNetCoreTool("tool", new DotNetCoreToolSettings
                {
                    ArgumentCustomization = args => args
                        .Append("uninstall")
                        .AppendSwitchQuoted("--tool-path", toolsPath.FullPath)
                        .AppendQuoted(package)
                });
        }

        if (!context.FileExists(toolPath))
        {
            context.DotNetCoreTool("tool", new DotNetCoreToolSettings
                {
                    ArgumentCustomization = args => args
                        .Append("install")
                        .AppendSwitchQuoted("--version", version)
                        .AppendSwitchQuoted("--tool-path", toolsPath.FullPath)
                        .AppendQuoted(package)
                });
        }

        if (!context.FileExists(toolPath))
        {
            throw new System.Exception($"Failed to install .NET Core tool {package} ({version}).");
        }

        return toolPath;
    }
}
