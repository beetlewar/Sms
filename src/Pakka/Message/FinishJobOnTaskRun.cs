using System;

namespace Pakka.Message
{
    public class FinishJobOnTaskRun : IMessage
    {
        public Guid ActorId { get; }
        public Guid JobId { get; }

        public FinishJobOnTaskRun(Guid actorId, Guid jobId)
        {
            ActorId = actorId;
            JobId = jobId;
        }
    }
}
