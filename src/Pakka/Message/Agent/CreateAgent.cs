using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Message
{
    public class CreateAgent : IMessage
    {
        public string ActorType => ActorTypes.Agent;
        public Guid ActorId { get; }

        public CreateAgent(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
