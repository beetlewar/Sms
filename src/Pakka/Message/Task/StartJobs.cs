using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Message
{
    public class StartJobs : IMessage
    {
        public Guid ActorId { get; }

        public string ActorType => ActorTypes.Task;

        public StartJobs(Guid actorId)
        {
            ActorId = actorId;
        }
    }
}
