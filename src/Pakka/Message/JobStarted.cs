using System;

namespace Pakka.Message
{
	public class JobStarted
	{
		public Guid Id { get; }

		public JobStarted(Guid id)
		{
			Id = id;
		}
	}
}
