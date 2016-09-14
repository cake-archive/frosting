using Cake.Core;
using Cake.Frosting;

public class Program : IFrostingStartup
{
    public static int Main(string[] args)
    {
        // Create the host.
        var host = new CakeHostBuilder()
            .WithArguments(args)
            .UseStartup<Program>()
            .Build();

        // Run the host.
        return host.Run();
    }

    public void Configure(ICakeServices services)
    {
        services.UseContext<BuildContext>();
        services.UseLifetime<BuildLifetime>();
        services.UseWorkingDirectory("..");
    }
}