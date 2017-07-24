using System;

namespace Pakka.Message
{
	public class StartJob
	{
		public Guid AgentId { get; }

		public Guid JobId { get; }

		public StartJob(Guid agentId, Guid jobId)
		{
			AgentId = agentId;
			JobId = jobId;
		}
	}
}
