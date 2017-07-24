using Pakka.Actor;
using Pakka.Port;
using System;
using System.Collections.Generic;

namespace Pakka.Tests.Stub
{
	public class DecomposerStub : IDecomposer
	{
		private readonly int _numJobs;
		private readonly Guid _agentId;

		public DecomposerStub(int numJobs, Guid agentId)
		{
			_numJobs = numJobs;
			_agentId = agentId;
		}

		public IEnumerable<Guid> Decompose()
		{
			for (var i = 0; i < _numJobs; i++)
			{
				yield return _agentId;
			}
		}
	}
}
