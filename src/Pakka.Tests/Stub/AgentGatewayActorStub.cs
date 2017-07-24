using Pakka.Actor;
using Pakka.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Tests.Stub
{
    public class AgentGatewayActorStub : IActor
    {
        public Guid Id { get; }

        public AgentGatewayActorStub(Guid id)
        {
            Id = id;
        }

        public IEnumerable<IMessage> Execute(IMessage message)
        {
            return When((dynamic)message);
        }

        private IEnumerable<IMessage> When(object message)
        {
            throw new InvalidOperationException();
        }

        private IEnumerable<IMessage> When(GatewayStartJob message)
        {
            yield return new GatewayJobEnqueued(message.AgentId, Id);
            yield return new GatewayJobStarted(message.AgentId, Id);
            yield return new GatewayJobFinished(message.AgentId, Id);
        }
    }
}
