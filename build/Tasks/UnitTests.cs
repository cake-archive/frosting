using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Frosting;

[Dependency(typeof(Build))]
public class UnitTests : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        var project = "./src/Cake.Frosting.Tests/Cake.Frosting.Tests.csproj";
        context.DotNetCoreTest(project, new DotNetCoreTestSettings {
            Configuration = context.Configuration,
            NoBuild = true,
            Verbose = false
        });
    }
}