using System;

namespace Pakka.Repository
{
	public interface IUnitOfWork
	{
		string ActorType { get; }

		IDisposable BeginTransaction();

		IActorRepository GetActorRepository();

		INotificationRepository GetNotificationRepository();
	}
}
