# Frosting

[![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/frosting/develop.svg)](https://ci.appveyor.com/project/cakebuild/frosting/branch/develop) 
[![MyGet](https://img.shields.io/myget/cake/vpre/Cake.Frosting.svg?label=myget)](https://www.myget.org/feed/cake/package/nuget/Cake.Frosting)

A .NET Core host for Cake, that allows you to write your build scripts as a 
portable console application (`netstandard1.0`). Frosting is currently 
in alpha, but more information, documentation and samples will be added soon.

**Expect things to move around initially. Especially naming of things.**

## Table of Contents

1. [Example](https://github.com/cake-build/frosting#example)
2. [Acknowledgement](https://github.com/cake-build/frosting#documentation)
3. [License](https://github.com/cake-build/frosting#license)

## Example

### 1. Add nuget.config

Start by adding a `nuget.config` file to your project.  
The reason for this is that Cake.Frosting is in preview at the moment and not
available on [nuget.org](https://nuget.org).

```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="myget.org" value="https://www.myget.org/F/cake/api/v3/index.json" protocolVersion="3" />
  </packageSources>
</configuration>
```

### 2. Add project.json

Now create a `project.json` file that will tell `dotnet` what dependencies are required
to build our program. The `Cake.Frosting` package will decide what version of `Cake.Core` and `Cake.Common`
you will get.

```json
{
  "version": "0.1.0-*",
  "buildOptions": {
    "emitEntryPoint": true
  },
  "dependencies": {
    "Cake.Frosting": "0.1.0-alpha0007",
    "Microsoft.NETCore.App": {
      "type": "platform",
      "version": "1.0.0"
    }
  },
  "frameworks": {
    "netcoreapp1.0": {
      "imports": "dnxcore50"
    }
  }
}
```

### 3. Add Program.cs

For the sake of keeping the example simple, all classes are listed after each other, 
but you should of course treat the source code of your build scripts like any other
code and divide them up in individual files.

```csharp
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

public class Program
{
    public static int Main(string[] args)
    {
        // Create the host.
        var host = new CakeHostBuilder()
            .WithArguments(args)
            .ConfigureServices(services =>
            {
                // Use a custom settings class.
                services.UseContext<MySettings>();
            })
            .Build();

        // Run the host.
        return host.Run();
    }
}

public class MySettings : FrostingContext
{
    public bool Magic { get; set; }

    public MySettings(ICakeContext context)
        : base(context)
    {
        // You could also use a CakeLifeTime<Settings>
        // to provide a Setup method to setup the context.
        Magic = context.Arguments.HasArgument("magic");
    }
}

[TaskName("Provide-Another-Name-Like-This")]
public class Build : FrostingTask<MySettings>
{
    public override bool ShouldRun(MySettings context)
    {
        // Don't run this task on OSX.
        return context.Environment.Platform.Family != PlatformFamily.OSX;
    }

    public override void Run(MySettings context)
    {
        context.Information("Magic: {0}", context.Magic);
    }
}

[Dependency(typeof(Build))]
public class Default : FrostingTask
{
    // If you don't inherit from the generic task
    // the standard ICakeContext will be provided.
}
``` 

### 4. Run it!

To execute the build, simply run it like any .NET Core application.  
In the example we provide the custom `--magic` argument. Notice that
we use `--` to separate the arguments to the `dotnet` command.

```powershell
> dotnet restore
> dotnet run -- --magic
```

## Acknowledgement

The API for configuring and running the host have been heavily influenced by 
the [ASP.NET Core hosting API](https://github.com/aspnet/Hosting).

## License

Copyright © Patrik Svensson, Mattias Karlsson, Gary Ewan Park and contributors.
Frosting is provided as-is under the MIT license. For more information see 
[LICENSE](https://github.com/cake-build/cake/blob/develop/LICENSE).

* For Autofac, see https://github.com/autofac/Autofac/blob/master/LICENSE

## Thanks

A big thank you has to go to [JetBrains](https://www.jetbrains.com) who provide 
each of the Cake developers with an 
[Open Source License](https://www.jetbrains.com/support/community/#section=open-source) 
for [ReSharper](https://www.jetbrains.com/resharper/) that helps with the development of Cake.

The Cake Team would also like to say thank you to the guys at
[MyGet](https://www.myget.org/) for their support in providing a Professional 
subscription which allows us to continue to push all of our pre-release 
editions of Cake NuGet packages for early consumption by the Cake community.

## Code of Conduct

This project has adopted the code of conduct defined by the 
[Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior 
in our community. For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](http://www.dotnetfoundation.org).
