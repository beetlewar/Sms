using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Message
{
    public class GatewayJobStarted : IMessage
    {
        public Guid ActorId { get; }

        public string ActorType => ActorTypes.Agent;

        public Guid JobId { get; }

        public GatewayJobStarted(Guid actorId, Guid jobId)
        {
            ActorId = actorId;
            JobId = jobId;
        }
    }
}
