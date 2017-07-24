using System;

namespace Pakka.Port
{
	public interface ITaskRunIdProvider
	{
		Guid GetByJobId(Guid jobId);
	}
}
