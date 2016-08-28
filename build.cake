#load "scripts/parameters.cake"
#load "scripts/version.cake"
#load "scripts/publish.cake"

var parameters = Parameters.Create(Context);

Setup(context => 
{
    parameters.Setup(context);

    Information("Version: {0}", parameters.Version);
    Information("Version suffix: {0}", parameters.Suffix);
    Information("Configuration: {0}", parameters.Configuration);
    Information("Target: {0}", parameters.Target);
    Information("AppVeyor: {0}", parameters.IsRunningOnAppVeyor);
});

Task("Update-Version-Information")
    .Does(context =>
{
    BuildVersion.UpdateVersion(parameters);
});

Task("Restore")
    .IsDependentOn("Update-Version-Information")
    .Does(context =>
{
    DotNetCoreRestore("./src", new DotNetCoreRestoreSettings
    {
        Verbose = false,
        Verbosity = DotNetCoreRestoreVerbosity.Warning,
        Sources = new [] {
            "https://www.myget.org/F/xunit/api/v3/index.json",
            "https://dotnet.myget.org/F/dotnet-core/api/v3/index.json",
            "https://dotnet.myget.org/F/cli-deps/api/v3/index.json",
            "https://api.nuget.org/v3/index.json",
            "https://www.myget.org/F/cake/api/v3/index.json"
        }
    });
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(context =>
{
    foreach(var path in parameters.Projects)
    {
        DotNetCoreBuild(path.FullPath, new DotNetCoreBuildSettings(){
            Configuration = "Release",
            VersionSuffix = parameters.Suffix
        });
    }
});

Task("Unit-Tests")
    .IsDependentOn("Build")
    .Does(context =>
{
    DotNetCoreTest("./src/Cake.Frosting.Tests", new DotNetCoreTestSettings {
        Configuration = "Release",
        NoBuild = true,
        Verbose = false
    });
});

Task("Package")
    .IsDependentOn("Unit-Tests")
    .Does(context =>
{
    DotNetCorePack("./src/Cake.Frosting/project.json", new DotNetCorePackSettings(){
        Configuration = "Release",
        VersionSuffix = parameters.Suffix,
        NoBuild = true,
        Verbose = false
    });
});

Task("Publish-MyGet")
    .IsDependentOn("Package")
    .WithCriteria(() => parameters.ShouldPublishToMyGet)
    .Does(context =>
{
    Publish(context, parameters, parameters.MyGetSource, parameters.MyGetApiKey);
});

Task("Default")
    .IsDependentOn("Package");

Task("AppVeyor")
    .WithCriteria(parameters.IsRunningOnAppVeyor)
    .IsDependentOn("Publish-MyGet");

RunTarget(parameters.Target);