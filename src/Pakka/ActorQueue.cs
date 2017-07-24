using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pakka.Repository;

namespace Pakka
{
	public class ActorQueue
	{
		private readonly object _syncTask = new object();
		private Task _lastTask;
		private readonly CancellationToken _token;
		private readonly ActorDispatcher _actorDispatcher;
		private readonly IUnitOfWork _unitOfWork;

		public ActorQueue(
			ActorDispatcher actorDispatcher,
			IUnitOfWork unitOfWork,
			CancellationToken token)
		{
			_actorDispatcher = actorDispatcher;
			_unitOfWork = unitOfWork;
			_token = token;

			_lastTask = Task.Run(() => { });
		}

		public Task Enqueue(Notification notification)
		{
			var notificationRepository = _unitOfWork.GetNotificationRepository();

			notificationRepository.Save(new[] {notification});

			lock (_syncTask)
			{
				_lastTask = _lastTask.ContinueWith(t =>
				{
					try
					{
						Execute(notification);
					}
					catch (Exception e)
					{
						Console.WriteLine(e);
					}
				}, _token);

				return _lastTask;
			}
		}

		private void Execute(Notification notification)
		{
			Notification[] newNotifications;

			using (_unitOfWork.BeginTransaction())
			{
				var notificationRepository = _unitOfWork.GetNotificationRepository();

				notificationRepository.MarkAsProcessed(notification.ActorType, notification.ActorId);

				var actorRepository = _unitOfWork.GetActorRepository();

				var actor = actorRepository.GetOrCreate(notification.ActorId);

				newNotifications = actor.Execute(notification.Message).ToArray();

				notificationRepository.Save(newNotifications);

				actorRepository.Update(actor);
			}

			foreach (var newNotification in newNotifications)
			{
				_actorDispatcher.Dispatch(newNotification);
			}
		}
	}
}
