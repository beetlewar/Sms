using System.Collections.Generic;
using System.Linq;

namespace Pakka.Repository
{
	public class ActorUnitOfWorkLocator
	{
		private readonly Dictionary<string, IUnitOfWork> _actorUnitOfWorks;

		public ActorUnitOfWorkLocator(params IUnitOfWork[] actorUnitOfWorks)
		{
			_actorUnitOfWorks = actorUnitOfWorks.ToDictionary(rep => rep.ActorType);
		}

		public IUnitOfWork Locate(string actorType)
		{
			return _actorUnitOfWorks[actorType];
		}

		public IEnumerable<IUnitOfWork> GetAll()
		{
			return _actorUnitOfWorks.Values;
		}
	}
}
