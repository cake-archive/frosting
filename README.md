# Frosting

[![NuGet](https://img.shields.io/nuget/v/Cake.Frosting.svg)](https://www.nuget.org/packages/Cake.Frosting) [![Azure Artifacts](https://azpkgsshield.azurevoodoo.net/cake-build/Cake/cake/cake.frosting)](https://dev.azure.com/cake-build/Cake/_packaging?_a=package&feed=cake&package=Cake.Frosting&protocolType=NuGet)

A .NET Core host for Cake, that allows you to write your build scripts as a 
(portable) console application (`netcoreapp3.1` or `net461`). 

## Table of Contents

1. [Example](https://github.com/cake-build/frosting#example)
2. [Acknowledgement](https://github.com/cake-build/frosting#acknowledgement)
3. [License](https://github.com/cake-build/frosting#license)
4. [Thanks](https://github.com/cake-build/frosting#thanks)
5. [Code of Conduct](https://github.com/cake-build/frosting#code-of-conduct)
6. [.NET Foundation](https://github.com/cake-build/frosting#net-foundation)

## Example

### 1. Install .NET Core SDK 3.1.301 or later

You can find the SDK at [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).

### 2. Install the template

```
> dotnet new --install Cake.Frosting.Template
```

### 3. Create a new Frosting project

Now it's time to create a new Frosting project.  
Go to the repository where you want to add a new Frosting build script and run the following command:

```
> dotnet new cakefrosting
```

### 4. Profit

Now you should be able to run your newly created builds script.  

```powershell
> ./build.ps1
```

**NOTE:** You're not supposed to commit the produced binaries to your repository.  
The above command is what you're expected to run from your bootstrapper.

## Building from source

### .NET Core SDK

To build from source, you will need to have 
[.NET Core SDK 3.1.301](https://dotnet.microsoft.com/download)
installed on your machine.

### Visual Studio 2019 (optional)

If you want to develop using Visual Studio, then you need to use Visual Studio 2019 (16.6) or higher.

## Acknowledgement

The API for configuring and running the host have been heavily influenced by 
the [ASP.NET Core hosting API](https://github.com/aspnet/Hosting).

## License

Copyright © [.NET Foundation](http://dotnetfoundation.org/) and contributors.  
Frosting is provided as-is under the MIT license. For more information see 
[LICENSE](https://github.com/cake-build/frosting/blob/develop/LICENSE).

## Thanks

A big thank you has to go to [JetBrains](https://www.jetbrains.com) who provide 
each of the Cake developers with an 
[Open Source License](https://www.jetbrains.com/support/community/#section=open-source) 
for [ReSharper](https://www.jetbrains.com/resharper/) that helps with the development of Cake.

## Code of Conduct

This project has adopted the code of conduct defined by the 
[Contributor Covenant](http://contributor-covenant.org/) to clarify expected behavior 
in our community. For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](http://www.dotnetfoundation.org).
