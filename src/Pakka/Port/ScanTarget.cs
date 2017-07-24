using System;

namespace Pakka.Port
{
	public class ScanTarget
	{
		public Guid AgentId { get; }

		public string Target { get; }

		public ScanTarget(Guid agentId, string target)
		{
			AgentId = agentId;
			Target = target;
		}
	}
}
