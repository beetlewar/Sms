using System;

namespace Pakka.Message
{
    public class CreateTask : IMessage
    {
        public string ActorType => ActorTypes.Task;
        public Guid ActorId { get; }

        public CreateTask(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
