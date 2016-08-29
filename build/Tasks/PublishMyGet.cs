using Cake.Common.Diagnostics;
using Cake.Common.Tools.NuGet;
using Cake.Common.Tools.NuGet.Push;
using Cake.Core;
using Cake.Frosting;

[Dependency(typeof(Package))]
public class PublishMyGet : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        if(context.MyGetSource == null)
        {
            throw new CakeException("MyGet source was not provided.");
        }
        if(context.MyGetApiKey == null)
        {
            throw new CakeException("MyGet API key was not provided.");
        }

        foreach(var project in context.Projects)
        {
            if(project.Publish) 
            {
                context.Information("Publishing {0} to MyGet...", project.Name);
                
                // Get the file paths.
                var root = project.Path.GetDirectory();
                var packageVersion = string.Concat(context.Version, "-", context.Suffix).Trim('-');
                var files = new[] {
                    root.FullPath + "/bin/Release/" + project.Name + "." + packageVersion + ".nupkg",
                    root.FullPath + "/bin/Release/" + project.Name + "." + packageVersion + ".symbols.nupkg"
                };

                foreach(var file in files) 
                {
                    context.NuGetPush(file, new NuGetPushSettings() {
                        Source = context.MyGetSource,
                        ApiKey = context.MyGetApiKey
                    });
                }
            }
        }
    }
}