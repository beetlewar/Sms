using System;

namespace Pakka.Message
{
    public class TaskRunFinished : IMessage
    {
        public Guid ActorId { get; }
        public Guid TaskRunId { get; }

        public TaskRunFinished(Guid actorId, Guid taskRunId)
        {
            ActorId = actorId;
            TaskRunId = taskRunId;
        }
    }
}
