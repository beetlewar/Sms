using System.Collections.Generic;

namespace Pakka.Actor
{
	public interface IActor
	{
		IEnumerable<Notification> Execute(object message);
	}
}
