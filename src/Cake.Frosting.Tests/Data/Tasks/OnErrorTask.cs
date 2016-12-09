using System;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Frosting.Tests.Data.Tasks
{
    [TaskName("On-Error")]
    public class OnErrorTask : FrostingTask<ICakeContext>
    {
        public override void Run(ICakeContext context)
        {
            throw new FrostingException("On test exception");
        }

        public override void OnError(Exception exception, ICakeContext context)
        {
            context.Log.Error(exception.Message);
        }
    }
}
