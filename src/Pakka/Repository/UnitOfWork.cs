using System;

namespace Pakka.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IActorRepository _actorRepository;
		private readonly INotificationRepository _notificationRepository;

		public string ActorType { get; }

		public UnitOfWork(
			string actorType,
			IActorRepository actorRepository, 
			INotificationRepository notificationRepository)
		{
			ActorType = actorType;
			_actorRepository = actorRepository;
			_notificationRepository = notificationRepository;
		}

		public IDisposable BeginTransaction()
		{
			return new EmptyDisposalbe();
		}

		public IActorRepository GetActorRepository()
		{
			return _actorRepository;
		}

		public INotificationRepository GetNotificationRepository()
		{
			return _notificationRepository;
		}

		private class EmptyDisposalbe : IDisposable
		{
			public void Dispose()
			{
			}
		}
	}
}
