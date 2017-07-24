using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
    public class Job
    {
        public Guid Id { get; }
        public JobState State { get; private set; }
        public Guid AgentId { get; private set; }

        public Job(Guid id, Guid agentId)
        {
            Id = id;
            AgentId = agentId;
        }

        public IMessage StartJob()
        {
            State = JobState.Enqueued;

            return new StartJob(AgentId, Id);
        }

        public void Enqueued()
        {
            State = JobState.Enqueued;
        }

        public void Started()
        {
            State = JobState.Running;
        }

        public void Finished()
        {
            State = JobState.Finished;
        }

        public enum JobState
        {
            Assigned,
            Enqueued,
            Running,
            Finishing,
            Finished
        }
    }
}
