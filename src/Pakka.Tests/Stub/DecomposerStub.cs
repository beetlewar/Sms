using Pakka.Actor;
using Pakka.Port;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Tests.Stub
{
    public class DecomposerStub : IDecomposer
    {
        private readonly int _numJobs;
        private readonly Guid _agentId;

        public DecomposerStub(int numJobs, Guid agentId)
        {
            _numJobs = numJobs;
            _agentId = agentId;
        }

        public IEnumerable<Job> Decompose()
        {
            for (int i = 0; i < _numJobs; i++)
            {
                yield return new Job(Guid.NewGuid(), _agentId);
            }
        }
    }
}
