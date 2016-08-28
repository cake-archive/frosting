#load "parameters.cake"

public static void Publish(ICakeContext context, Parameters parameters, string source, string apiKey)
{
    if(source == null)
    {
        throw new CakeException("NuGet source was not provided.");
    }
    if(apiKey == null)
    {
        throw new CakeException("NuGet API key was not provided.");
    }

    var packageVersion = string.Concat(parameters.Version, "-", parameters.Suffix).Trim('-');
    var files = new[] {
        "./src/Cake.Frosting/bin/Release/Cake.Frosting." + packageVersion + ".nupkg",
        "./src/Cake.Frosting/bin/Release/Cake.Frosting." + packageVersion + ".symbols.nupkg"
    };

    foreach(var file in files) 
    {
        context.NuGetPush(file, new NuGetPushSettings() {
            Source = source,
            ApiKey = apiKey
        });
    }
}