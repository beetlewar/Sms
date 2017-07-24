using System;
using Pakka.Port;

namespace Pakka.Message
{
	public class JobResult
	{
		public Guid Id { get; }
		public ScanTarget[] Targets { get; }

		public JobResult(Guid id, ScanTarget[] targets)
		{
			Id = id;
			Targets = targets;
		}
	}
}
