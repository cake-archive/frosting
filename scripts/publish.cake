#load "parameters.cake"
#load "project.cake"

public static void Publish(ICakeContext context, Parameters parameters, Project project, string source, string apiKey)
{
    if(source == null)
    {
        throw new CakeException("NuGet source was not provided.");
    }
    if(apiKey == null)
    {
        throw new CakeException("NuGet API key was not provided.");
    }

    var root = project.Path.GetDirectory();
    var packageVersion = string.Concat(parameters.Version, "-", parameters.Suffix).Trim('-');
    var files = new[] {
        root.FullPath + "/bin/Release/" + project.Name + "." + packageVersion + ".nupkg",
        root.FullPath + "/bin/Release/" + project.Name + "." + packageVersion + ".symbols.nupkg"
    };

    foreach(var file in files) 
    {
        context.NuGetPush(file, new NuGetPushSettings() {
            Source = source,
            ApiKey = apiKey
        });
    }
}