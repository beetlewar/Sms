using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Pakka.Actor;
using Pakka.Message;
using Pakka.Port;
using Pakka.Repository;
using Xunit;
using Pakka.Tests.Stub;

namespace Pakka.Tests
{
	public class UseCaseTests
	{
		[Fact]
		public void Pentest()
		{
			var agentId = Guid.NewGuid();
			var decomposer = new DecomposerStub(agentId).WithTargets(2);

			var taskId = Guid.NewGuid();
			var testContext = new TestContext(decomposer, new AgentGatewayActorRepositoryStub())
				.WithAgent(agentId)
				.WithTask(taskId);

			var taskRunId = Guid.NewGuid();

			testContext.Dispatch(new Notification(ActorTypes.Task, taskId, new RunTask(taskId, taskRunId, false)));

			Assert.True(testContext.WaitTaskRunFinished(taskRunId, 5));
		}

		[Fact]
		public void Abc()
		{
			var agentId = Guid.NewGuid();
			var decomponser = new DecomposerStub(agentId).WithHD();

			var hdScanTargets = new[]
			{
				new ScanTarget(agentId, "192.168.2.0"),
				new ScanTarget(agentId, "192.168.2.1"),
				new ScanTarget(agentId, "192.168.2.2"),
			};

			var actorRepositoryStub = new AgentGatewayActorRepositoryStub()
				.WithCreateActorFunc(
					id => new AgentGatewayActorStub(id).WithJobResultsFunc(sj => GetHDJobResults(sj, hdScanTargets)));

			var taskId = Guid.NewGuid();
			var testContext = new TestContext(decomponser, actorRepositoryStub)
				.WithAgent(agentId)
				.WithTask(taskId);

			var taskRunId = Guid.NewGuid();

			testContext.Dispatch(new Notification(ActorTypes.Task, taskId, new RunTask(taskId, taskRunId, true)));

			Assert.True(testContext.WaitTaskRunFinished(taskRunId, 5));
			Assert.True(testContext.WaitJobsFinished(taskRunId, hdScanTargets, 5));
		}

		private IEnumerable<JobResult> GetHDJobResults(StartJob startJob, ScanTarget[] scanTargets)
		{
			return scanTargets.Select(st => new JobResult(startJob.JobId, new[] {st}));
		}
	}
}
