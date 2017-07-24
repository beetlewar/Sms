using System;

namespace Pakka.Message
{
	public class TaskRunFinished
	{
		public Guid TaskRunId { get; }

		public TaskRunFinished(Guid taskRunId)
		{
			TaskRunId = taskRunId;
		}
	}
}
