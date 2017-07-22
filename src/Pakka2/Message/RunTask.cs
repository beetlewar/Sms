using System;

namespace Pakka.Message
{
    public class RunTask : IMessage
    {
        public Guid ActorId { get; }
        public Guid AgentId { get; }
        public int NumJobs { get; }

        public RunTask(Guid actorId, Guid agentId, int numJobs)
        {
            ActorId = actorId;
            AgentId = agentId;
            NumJobs = numJobs;
        }
    }
}
