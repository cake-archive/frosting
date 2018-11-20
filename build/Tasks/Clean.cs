using Cake.Common.IO;
using Cake.Frosting;

public class Clean : FrostingTask<Context>
{
    public override void Run(Context context)
    {
        var directories = context.GetDirectories("./src/**/bin") 
            + context.GetDirectories("./src/**/obj")
            + context.Artifacts;

        foreach (var directory in directories)
        {
            context.CleanDirectory(directory);
        }
    }
}