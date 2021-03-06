﻿using System;
using Pakka.Actor;
using Pakka.Port;
using System.Collections.Concurrent;

namespace Pakka.Repository
{
	public class AgentRepository : IActorRepository
	{
		private readonly ConcurrentDictionary<Guid, Agent> _agents = new ConcurrentDictionary<Guid, Agent>();

		private readonly ITaskRunIdProvider _taskIdProvider;

		public AgentRepository(ITaskRunIdProvider taskIdProvider)
		{
			_taskIdProvider = taskIdProvider;
		}

		public IActor GetOrCreate(Guid id)
		{
			Agent agent;
			if (!_agents.TryGetValue(id, out agent))
			{
				agent = new Agent(id, _taskIdProvider);
				_agents[id] = agent;
			}

			return _agents[id];
		}

		public void Update(IActor actor)
		{
			var agent = (Agent) actor;

			_agents[agent.Id] = agent;
		}
	}
}
