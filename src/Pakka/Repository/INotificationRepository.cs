using System;
using System.Collections.Generic;

namespace Pakka.Repository
{
	public interface INotificationRepository
	{
		void Save(IEnumerable<Notification> notifications);

		IEnumerable<Notification> GetPendingNotifications(string actorType);

		void MarkAsProcessed(string actorType, Guid id);
	}
}
