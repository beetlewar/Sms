using System;
using System.Collections.Generic;
using System.Threading;
using Pakka.Repository;

namespace Pakka
{
	public class ActorDispatcher
	{
		private readonly object _sync = new object();
		private readonly Dictionary<Tuple<string, Guid>, ActorQueue> _actorQueues;
		private readonly ActorUnitOfWorkLocator _actoryRepositoryLocator;
		private readonly CancellationToken _token;

		public ActorDispatcher(ActorUnitOfWorkLocator actoryRepositoryLocator, CancellationToken token)
		{
			_actorQueues = new Dictionary<Tuple<string, Guid>, ActorQueue>();
			_actoryRepositoryLocator = actoryRepositoryLocator;
			_token = token;
		}

		public void Dispatch(Notification notification)
		{
			ActorQueue actorQueue;
			var key = new Tuple<string, Guid>(notification.ActorType, notification.ActorId);

			lock (_sync)
			{
				if (!_actorQueues.TryGetValue(key, out actorQueue))
				{
					var repository = _actoryRepositoryLocator.Locate(notification.ActorType);
					actorQueue = new ActorQueue(this, repository, _token);
					_actorQueues[key] = actorQueue;
				}
			}

			actorQueue.Enqueue(notification);
		}

		public void Restore()
		{
			foreach (var unitOfWorkLocator in _actoryRepositoryLocator.GetAll())
			{
				RestoreNotifications(unitOfWorkLocator.ActorType, unitOfWorkLocator.GetNotificationRepository());
			}
		}

		private void RestoreNotifications(string actorType, INotificationRepository notificationRepository)
		{
			foreach (var pendingNotification in notificationRepository.GetPendingNotifications(actorType))
			{
				Dispatch(pendingNotification);
			}
		}
	}
}
