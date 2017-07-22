using System;

namespace Pakka.Message
{
    public class JobStarted : IMessage
    {
        public Guid ActorId { get; }

        public JobStarted(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
