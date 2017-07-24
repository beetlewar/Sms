using System;

namespace Pakka.Message
{
    public class StartJob : IMessage
    {
        public string ActorType => ActorTypes.Agent;
        public Guid ActorId { get; }
        public Guid JobId { get; }

        public StartJob(Guid actorId, Guid jobId)
        {
            ActorId = actorId;
            JobId = jobId;
        }
    }
}
