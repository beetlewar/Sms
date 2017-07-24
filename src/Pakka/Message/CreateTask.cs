using System;

namespace Pakka.Message
{
	public class CreateTask
	{
		public Guid Id { get; }

		public CreateTask(Guid id)
		{
			Id = id;
		}
	}
}
