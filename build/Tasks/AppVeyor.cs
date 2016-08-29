using Cake.Frosting;

[Dependency(typeof(PublishMyGet))]
public class AppVeyor : FrostingTask<BuildContext>
{
    public override bool ShouldRun(BuildContext context)
    {
        return context.AppVeyor;
    } 
}