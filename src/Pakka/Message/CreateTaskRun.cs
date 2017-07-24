using System;

namespace Pakka.Message
{
	public class CreateTaskRun
	{
		public Guid Id { get; }

		public Guid TaskId { get; }

		public CreateTaskRun(Guid id, Guid taskId)
		{
			Id = id;
			TaskId = taskId;
		}
	}
}
