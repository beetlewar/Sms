using System;

namespace Pakka.Message
{
    public class JobComplete : IMessage
    {
        public Guid ActorId { get; }

        public JobComplete(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
