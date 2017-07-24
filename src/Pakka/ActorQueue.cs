using System;
using System.Threading;
using System.Threading.Tasks;
using Pakka.Repository;

namespace Pakka
{
	public class ActorQueue
	{
		private readonly object _syncTask = new object();
		private Task _lastTask;
		private readonly IActorRepository _repository;
		private readonly CancellationToken _token;
		private readonly ActorDispatcher _actorDispatcher;

		public ActorQueue(
			ActorDispatcher actorDispatcher,
			IActorRepository repository,
			CancellationToken token)
		{
			_actorDispatcher = actorDispatcher;
			_repository = repository;
			_token = token;

			_lastTask = Task.Run(() => { });
		}

		public void Enqueue(Notification notification)
		{
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
			}
		}

		private void Execute(Notification notification)
		{
			var actor = _repository.GetOrCreate(notification.ActorId);

			var notifications = actor.Execute(notification.Message);

			_repository.Update(actor);

			foreach (var newNotification in notifications)
			{
				_actorDispatcher.Dispatch(newNotification);
			}
		}
	}
}
