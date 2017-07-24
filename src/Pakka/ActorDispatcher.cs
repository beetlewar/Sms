using System;
using System.Collections.Concurrent;
using System.Threading;
using Pakka.Repository;

namespace Pakka
{
	public class ActorDispatcher
	{
		private readonly ConcurrentDictionary<Tuple<string, Guid>, ActorQueue> _actorQueues;
		private readonly ActorRepositoryLocator _actoryRepositoryLocator;
		private readonly CancellationToken _token;

		public ActorDispatcher(ActorRepositoryLocator actoryRepositoryLocator, CancellationToken token)
		{
			_actorQueues = new ConcurrentDictionary<Tuple<string, Guid>, ActorQueue>();
			_actoryRepositoryLocator = actoryRepositoryLocator;
			_token = token;
		}

		public void Dispatch(Notification notification)
		{
			ActorQueue actorQueue;
			var key = new Tuple<string, Guid>(notification.ActorType, notification.ActorId);
			if (!_actorQueues.TryGetValue(key, out actorQueue))
			{
				var repository = _actoryRepositoryLocator.Locate(notification.ActorType);
				actorQueue = new ActorQueue(this, repository, _token);
				_actorQueues[key] = actorQueue;
			}

			actorQueue.Enqueue(notification);
		}
	}
}
