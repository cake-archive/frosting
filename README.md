# Frosting

[![AppVeyor branch](https://img.shields.io/appveyor/ci/cakebuild/frosting/develop.svg)](https://ci.appveyor.com/project/cakebuild/frosting/branch/develop) 
[![MyGet](https://img.shields.io/myget/cake/vpre/Cake.Frosting.svg?label=myget)](https://www.myget.org/feed/cake/package/nuget/Cake.Frosting)

A .NET Core host for Cake, that allows you to write your build scripts as a 
(portable) console application (`netcoreapp1.1` or `net461`). Frosting is currently 
in alpha, but more information, documentation and samples will be added soon.

**Expect things to move around initially. Especially naming of things.**

## Table of Contents

1. [Example](https://github.com/cake-build/frosting#example)
2. [Acknowledgement](https://github.com/cake-build/frosting#acknowledgement)
3. [License](https://github.com/cake-build/frosting#license)
4. [Thanks](https://github.com/cake-build/frosting#thanks)
5. [Code of Conduct](https://github.com/cake-build/frosting#code-of-conduct)
6. [.NET Foundation](https://github.com/cake-build/frosting#net-foundation)

## Example

### 1. Install .NET Core SDK 1.0.4 or later

You can find the SDK at [https://www.microsoft.com/net/download/core](https://www.microsoft.com/net/download/core).

### 2. Install the template

Cake.Frosting is currently in preview, so you will have to specify the
template version explicitly.

```
> dotnet new --install Cake.Frosting.Template::0.1.0-*
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
[.NET Core SDK 1.0.4](https://www.microsoft.com/net/download/core)
installed on your machine.

### Visual Studio 2017 (optional)

If you want to develop using Visual Studio, then you need to use Visual Studio 2017 (15.2) or higher.

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
