using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Message
{
    public class JobFinished : IMessage
    {
        public Guid ActorId { get; }

        public Guid JobId { get; }

        public string ActorType => ActorTypes.Task;

        public JobFinished(Guid actorId, Guid jobId)
        {
            ActorId = actorId;
            JobId = jobId;
        }
    }
}
