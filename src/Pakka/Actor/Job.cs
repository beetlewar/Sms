using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
	public class Job
	{
		public Guid Id { get; }

		public JobState State { get; private set; }

		public Guid AgentId { get; }

		public string Target { get; }

		public Job(Guid id, Guid agentId, string target)
		{
			Id = id;
			AgentId = agentId;
			Target = target;
		}

		public Notification StartJob()
		{
			State = JobState.Enqueued;

			return new Notification(ActorTypes.Agent, AgentId, new StartJob(AgentId, Id));
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
