using System;

namespace Pakka.Message
{
	public class CreateTaskRun
	{
		public Guid Id { get; }

		public Guid TaskId { get; }

		public bool IsAbc { get; }

		public CreateTaskRun(Guid id, Guid taskId, bool isAbc)
		{
			Id = id;
			TaskId = taskId;
			IsAbc = isAbc;
		}
	}
}
