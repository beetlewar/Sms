using System;
using System.Collections.Generic;

namespace Pakka.Repository
{
	public class NotificationRepository : INotificationRepository
	{
		private readonly object _sync = new object();

		private readonly Dictionary<Tuple<string, Guid>, List<Notification>> _notifications =
			new Dictionary<Tuple<string, Guid>, List<Notification>>();

		public void Save(IEnumerable<Notification> notifications)
		{
			lock (_sync)
			{
				foreach (var notification in notifications)
				{
					var key = new Tuple<string, Guid>(notification.ActorType, notification.ActorId);
					List<Notification> existingNotifications;
					if (!_notifications.TryGetValue(key, out existingNotifications))
					{
						existingNotifications = new List<Notification>();
						_notifications.Add(key, existingNotifications);
					}

					existingNotifications.Add(notification);
				}
			}
		}

		public IEnumerable<Notification> GetPendingNotifications(string actorType)
		{
			yield break;
		}

		public void MarkAsProcessed(string actorType, Guid id)
		{
		}
	}
}
