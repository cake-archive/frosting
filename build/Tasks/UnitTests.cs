using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Frosting;

[Dependency(typeof(Build))]
public class UnitTests : FrostingTask<BuildContext>
{
    public override void Run(BuildContext context)
    {
        context.DotNetCoreTest("./src/Cake.Frosting.Tests", new DotNetCoreTestSettings {
            Configuration = "Release",
            NoBuild = true,
            Verbose = false
        });
    }
}