#tool "nuget:https://www.nuget.org/api/v2?package=GitVersion.CommandLine&version=3.6.2"
#addin "nuget:?package=Newtonsoft.Json&version=9.0.1"

public class BuildVersion
{
    public string Version { get; private set; }
    public string Suffix { get; private set; }

    public BuildVersion(string version, string suffix)
    {
        Version = version;
        Suffix = suffix;

        if(string.IsNullOrWhiteSpace(Suffix))
        {
            Suffix = null;
        }
    }

    public static BuildVersion GetVersion(ICakeContext context, Parameters parameters)
    {
        string version = null;
        string semVersion = null;

        if (context.IsRunningOnWindows())
        {
            context.Information("Calculating semantic version...");
            if (!parameters.IsLocalBuild)
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
        }

        if (string.IsNullOrEmpty(version) || string.IsNullOrEmpty(semVersion))
        {
            context.Information("Fetching verson from first project.json...");
            foreach(var project in parameters.Projects) 
            {
                var content = System.IO.File.ReadAllText(project.FullPath, Encoding.UTF8);
                var node = Newtonsoft.Json.Linq.JObject.Parse(content);
                if(node["version"] != null) 
                {
                    version = node["version"].ToString().Replace("-*", "");
                }
            }
            semVersion = version;
        }

        if(string.IsNullOrWhiteSpace(version))
        {
            throw new CakeException("Could not calculate version of build.");
        }

        return new BuildVersion(version, semVersion.Substring(version.Length).TrimStart('-'));
    }

    public static void UpdateVersion(Parameters parameters)
    {
        foreach(var path in parameters.Projects)
        {
            var content = System.IO.File.ReadAllText(path.FullPath, Encoding.UTF8);
            var node = Newtonsoft.Json.Linq.JObject.Parse(content);
            if(node["version"] != null && node["version"].ToString() != (parameters.Version + "-*"))
            {
                node["version"].Replace(string.Concat(parameters.Version, "-*"));
                System.IO.File.WriteAllText(path.FullPath, node.ToString(), Encoding.UTF8);
            };
        }
    }
}