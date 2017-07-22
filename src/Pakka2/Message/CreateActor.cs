using System;

namespace Pakka.Message
{
    public class CreateActor : IMessage
    {
        public string ActorType { get; }
        public Guid ActorId { get; }

        public CreateActor(string actorType, Guid actorId)
        {
            ActorType = actorType;
            ActorId = actorId;
        }
    }
}
