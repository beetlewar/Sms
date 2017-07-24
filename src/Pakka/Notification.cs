using System;

namespace Pakka
{
	public class Notification
	{
		public string ActorType { get; }

		public Guid ActorId { get; }

		public object Message { get; }

		public Notification(string actorType, Guid actorId, object message)
		{
			ActorType = actorType;
			ActorId = actorId;
			Message = message;
		}
	}
}
