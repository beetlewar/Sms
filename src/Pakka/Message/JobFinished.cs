using System;

namespace Pakka.Message
{
	public class JobFinished
	{
		public Guid Id { get; }

		public JobFinished(Guid id)
		{
			Id = id;
		}
	}
}
