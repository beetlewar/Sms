using Pakka.Repository;
using System;
using Pakka.Actor;
using System.Collections.Concurrent;

namespace Pakka.Tests.Stub
{
	public class AgentGatewayActorRepositoryStub : IActorRepository
	{
		private readonly ConcurrentDictionary<Guid, AgentGatewayActorStub> _stubs =
			new ConcurrentDictionary<Guid, AgentGatewayActorStub>();

		public string ActorType => ActorTypes.AgentJobGateway;

		public Func<Guid, AgentGatewayActorStub> CreateActorFunc { get; private set; }

		public AgentGatewayActorRepositoryStub()
		{
			CreateActorFunc = id => new AgentGatewayActorStub(id);
		}

		public AgentGatewayActorRepositoryStub WithCreateActorFunc(Func<Guid, AgentGatewayActorStub> func)
		{
			CreateActorFunc = func;
			return this;
		}

		public IActor GetOrCreate(Guid id)
		{
			AgentGatewayActorStub stub;
			if (!_stubs.TryGetValue(id, out stub))
			{
				stub = CreateActorFunc(id);
				_stubs[id] = stub;
			}

			return _stubs[id];
		}

		public void Update(IActor actor)
		{
			var stub = (AgentGatewayActorStub) actor;

			_stubs[stub.Id] = stub;
		}
	}
}
