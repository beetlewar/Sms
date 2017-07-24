using System;

namespace Pakka.Message
{
	public class RunTask
	{
		public Guid Id { get; }

		public RunTask(Guid id)
		{
			Id = id;
		}
	}
}
