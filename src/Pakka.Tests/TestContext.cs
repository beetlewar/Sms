using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Pakka.Actor;
using Pakka.Port;
using Pakka.Repository;
using Pakka.Tests.Stub;

namespace Pakka.Tests
{
	public class TestContext
	{
		private readonly AgentRepository _agentRepository;

		private readonly TaskRepository _taskRepository;

		private readonly TaskRunRepository _taskRunRepository;

		private readonly ActorDispatcher _actorDispatcher;

		public TestContext(IDecomposer decomposer, AgentGatewayActorRepositoryStub agentGatewayActorRepositoryStub)
		{
			_taskRunRepository = new TaskRunRepository(decomposer);
			_taskRepository = new TaskRepository();
			_agentRepository = new AgentRepository(_taskRunRepository);

			var actorRepositoryLocator = new ActorRepositoryLocator(
				_agentRepository,
				_taskRepository,
				_taskRunRepository,
				agentGatewayActorRepositoryStub);

			_actorDispatcher = new ActorDispatcher(
				actorRepositoryLocator,
				CancellationToken.None);
		}

		public TestContext WithAgent(Guid id)
		{
			_agentRepository.GetOrCreate(id);

			return this;
		}

		public TestContext WithTask(Guid id)
		{
			_taskRunRepository.GetOrCreate(id);

			return this;
		}

		public bool WaitTaskRunFinished(
			Guid taskRunId,
			int seconds)
		{
			return Retry(seconds, () =>
			{
				var taskRun = (TaskRun) _taskRunRepository.GetOrCreate(taskRunId);
				if (taskRun.State != TaskRun.TaskRunState.Finished)
				{
					return false;
				}

				var task = (Task) _taskRepository.GetOrCreate(taskRun.TaskId);
				return task.TaskRunId.HasValue && task.State == Task.TaskState.Idle;
			});
		}

		private static bool Retry(int seconds, Func<bool> f)
		{
			for (var i = 0; i < seconds; i++)
			{
				Thread.Sleep(TimeSpan.FromSeconds(1));

				if (f())
				{
					return true;
				}
			}

			return false;
		}

		public bool WaitJobsFinished(
			Guid taskRunId,
			IEnumerable<ScanTarget> jobScanTargets,
			int seconds)
		{
			return Retry(seconds, () =>
			{
				var taskRun = (TaskRun) _taskRunRepository.GetOrCreate(taskRunId);
				return jobScanTargets.All(sc => HasJobWithScanTarget(sc, taskRun.Jobs));
			});
		}

		private static bool HasJobWithScanTarget(ScanTarget scanTarget, IEnumerable<Job> jobs)
		{
			foreach (var job in jobs)
			{
				if (job.AgentId == scanTarget.AgentId && job.Target == scanTarget.Target)
				{
					return true;
				}
			}
			return false;
		}

		public void Dispatch(Notification notification)
		{
			_actorDispatcher.Dispatch(notification);
		}
	}
}
