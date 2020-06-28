using Cake.Frosting;

[Dependency(typeof(AppVeyorArtifacts))]
[Dependency(typeof(PublishAzureArtifacts))]
public class AppVeyor : FrostingTask<Context>
{
    public override bool ShouldRun(Context context)
    {
        return context.AppVeyor;
    }
}
