using Pakka.Actor;
using Pakka.Port;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pakka.Tests.Stub
{
	public class DecomposerStub : IDecomposer
	{
		public Guid AgentId { get; }

		public ScanTarget[] ScanTargets { get; private set; }

		public DecomposerStub(Guid agentId)
		{
			AgentId = agentId;
			ScanTargets = new ScanTarget[0];
		}

		public DecomposerStub WithTargets(int count)
		{
			ScanTargets = CreateScanTargets(count).ToArray();

			return this;
		}

		private IEnumerable<ScanTarget> CreateScanTargets(int count)
		{
			for (var i = 0; i < count; i++)
			{
				yield return new ScanTarget(AgentId, "192.168.0." + (i + 1));
			}
		}

		public DecomposerStub WithHD()
		{
			ScanTargets = new[] {new ScanTarget(AgentId, "192.168.1.1")};

			return this;
		}

		public IEnumerable<ScanTarget> Decompose()
		{
			return ScanTargets;
		}
	}
}
