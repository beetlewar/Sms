using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pakka.Repository
{
    public class ActorRepositoryLocator
    {
        private readonly Dictionary<string, IActorRepository> _actorRepositories;

        public ActorRepositoryLocator(params IActorRepository[] actorRepositories)
        {
            _actorRepositories = actorRepositories.ToDictionary(rep => rep.ActorType);
        }

        public IActorRepository Locate(string actorType)
        {
            return _actorRepositories[actorType];
        }
    }
}
