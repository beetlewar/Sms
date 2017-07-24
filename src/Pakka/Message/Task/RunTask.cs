using System;

namespace Pakka.Message
{
    public class RunTask : IMessage
    {
        public string ActorType => ActorTypes.Task;
        public Guid ActorId { get; }

        public RunTask(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
