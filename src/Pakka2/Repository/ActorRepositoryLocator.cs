using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Pakka.Repository
{
    public class ActorRepositoryLocator
    {
        private readonly Dictionary<string, IActorRepository> _actorRepositories;

        public static ActorRepositoryLocator All()
        {
            var repositories =
                typeof(ActorRepositoryLocator)
                .GetTypeInfo()
                .Assembly
                .DefinedTypes
                .Where(t => t.ImplementedInterfaces.Any(i => i == typeof(IActorRepository)))
                .Select(t => Activator.CreateInstance(t.AsType()))
                .Cast<IActorRepository>();

            return new ActorRepositoryLocator(repositories);
        }

        public ActorRepositoryLocator(IEnumerable<IActorRepository> actorRepositories)
        {
            _actorRepositories = actorRepositories.ToDictionary(rep => rep.ActorType);
        }

        public IActorRepository Locate(string actorType)
        {
            return _actorRepositories[actorType];
        }
    }
}
