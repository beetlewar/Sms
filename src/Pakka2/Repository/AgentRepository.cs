using System;
using System.Collections.Generic;
using Pakka.Actor;

namespace Pakka.Repository
{
    public class AgentRepository : IActorRepository
    {
        private readonly Dictionary<Guid, Agent> _agents = new Dictionary<Guid, Agent>();

        public string ActorType => ActorTypes.Agent;

        public IActor Create(Guid id)
        {
            var agent = new Agent(id);
            _agents.Add(id, agent);
            return agent;
        }

        public IActor Get(Guid id)
        {
            return _agents[id];
        }

        public void Update(IActor actor)
        {
            var agent = (Agent)actor;

            _agents[agent.Id] = agent;
        }
    }
}
