using System;

namespace Pakka.Message
{
    public class Decompose : IMessage
    {
        public string ActorType => ActorTypes.Task;
        public Guid ActorId { get; }

        public Decompose(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
