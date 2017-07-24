using Pakka.Actor;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Pakka.Message;

namespace Pakka.Tests.Stub
{
	public class AgentGatewayActorStub : IActor
	{
		public Guid Id { get; }

		public Func<StartJob, IEnumerable<JobResult>> JobResultsFunc { get; private set; }

		public AgentGatewayActorStub(Guid id)
		{
			Id = id;
			JobResultsFunc = GetEmptyJobResults;
		}

		public AgentGatewayActorStub WithJobResultsFunc(Func<StartJob, IEnumerable<JobResult>> jobResultsFunc)
		{
			JobResultsFunc = jobResultsFunc;

			return this;
		}

		private IEnumerable<JobResult> GetEmptyJobResults(StartJob startJob)
		{
			yield break;
		}

		public IEnumerable<Notification> Execute(object message)
		{
			return When((dynamic) message);
		}

		private IEnumerable<Notification> When(object message)
		{
			throw new InvalidOperationException();
		}

		private IEnumerable<Notification> When(StartJob message)
		{
			yield return new Notification(ActorTypes.Agent, message.AgentId, new JobEnqueued(Id));
			yield return new Notification(ActorTypes.Agent, message.AgentId, new JobStarted(Id));

			foreach (var jobResult in JobResultsFunc(message))
			{
				yield return new Notification(ActorTypes.Agent, message.AgentId, jobResult);
			}

			yield return new Notification(ActorTypes.Agent, message.AgentId, new JobFinished(Id));
		}
	}
}
