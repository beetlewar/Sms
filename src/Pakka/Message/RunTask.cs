using System;

namespace Pakka.Message
{
	public class RunTask
	{
		public Guid Id { get; }

		public Guid TaskRunId { get; }

		public bool IsAbc { get; }

		public RunTask(Guid id, Guid taskRunId, bool isAbc)
		{
			Id = id;
			TaskRunId = taskRunId;
			IsAbc = isAbc;
		}
	}
}
