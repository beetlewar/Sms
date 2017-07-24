using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Message
{
    public class GatewayStartJob : IMessage
    {
        public Guid ActorId { get; }

        public Guid AgentId { get; }

        public string ActorType => ActorTypes.AgentJobGateway;

        public GatewayStartJob(Guid actorId, Guid agentId)
        {
            ActorId = actorId;
            AgentId = agentId;
        }
    }
}
