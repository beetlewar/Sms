using System;

namespace Pakka.Message
{
    public class AssignJobAgent : IMessage
    {
        public Guid ActorId { get; }
        public Guid TaskRunId { get; }
        public Guid AgentId { get; set; }

        public AssignJobAgent(Guid actorId, Guid taskRunId, Guid agentId)
        {
            ActorId = actorId;
            TaskRunId = taskRunId;
            AgentId = agentId;
        }
    }
}
