using Pakka.Actor;
using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Tests.Stub
{
	public class AgentGatewayActorStub : IActor
	{
		public Guid Id { get; }

		public AgentGatewayActorStub(Guid id)
		{
			Id = id;
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
			yield return new Notification(ActorTypes.Agent, message.AgentId, new JobFinished(Id));
		}
	}
}
