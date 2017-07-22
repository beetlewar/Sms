using System;

namespace Pakka.Message
{
    public class SetTaskWaiting : IMessage
    {
        public Guid ActorId { get; }

        public SetTaskWaiting(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
