using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Test;
using Cake.Frosting;

[Dependency(typeof(Build))]
public class UnitTests : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        foreach(var framework in new[] { "netcoreapp2.1", "netcoreapp3.0", "net461" })
        {
            var project = "./src/Cake.Frosting.Tests/Cake.Frosting.Tests.csproj";

            context.DotNetCoreTest(project, new DotNetCoreTestSettings {
                Framework = framework,
                Configuration = context.BuildConfiguration,
                NoBuild = true,
                NoRestore = true
            });
        }
    }
}
