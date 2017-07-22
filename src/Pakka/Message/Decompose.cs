using System;

namespace Pakka.Message
{
    public class Decompose : IMessage
    {
        public Guid ActorId { get; }
        public Guid TaskId { get; }
        public Guid AgentId { get; }
        public int NumJobs { get; }

        public Decompose(Guid actorId, Guid taskId, Guid agentId, int numJobs)
        {
            ActorId = actorId;
            TaskId = taskId;
            AgentId = agentId;
            NumJobs = numJobs;
        }
    }
}
