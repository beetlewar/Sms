using System;

namespace Pakka.Message
{
	public class TaskRunCreated
	{
		public Guid TaskRunId { get; }

		public TaskRunCreated(Guid taskRunId)
		{
			TaskRunId = taskRunId;
		}
	}
}
