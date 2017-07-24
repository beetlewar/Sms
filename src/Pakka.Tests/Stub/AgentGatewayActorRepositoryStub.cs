using Pakka.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pakka.Actor;
using System.Collections.Concurrent;

namespace Pakka.Tests.Stub
{
    public class AgentGatewayActorRepositoryStub : IActorRepository
    {
        private readonly ConcurrentDictionary<Guid, AgentGatewayActorStub> _stubs = new ConcurrentDictionary<Guid, AgentGatewayActorStub>();

        public string ActorType => ActorTypes.AgentJobGateway;

        public IActor GetOrCreate(Guid id)
        {
            AgentGatewayActorStub stub;
            if (!_stubs.TryGetValue(id, out stub))
            {
                stub = new AgentGatewayActorStub(id);
                _stubs[id] = stub;
            }

            return _stubs[id];
        }

        public void Update(IActor actor)
        {
            var stub = (AgentGatewayActorStub)actor;

            _stubs[stub.Id] = stub;
        }
    }
}
