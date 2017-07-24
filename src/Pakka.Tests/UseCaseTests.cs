using System;
using System.Threading;
using Pakka.Actor;
using Pakka.Message;
using Pakka.Repository;
using Xunit;
using Pakka.Tests.Stub;

namespace Pakka.Tests
{
	public class UseCaseTests
	{
		private readonly ActorRepositoryLocator _actorRepositoryLocator;
		private readonly CancellationTokenSource _tokenSource;
		private readonly ActorDispatcher _actorDispatcher;

		public UseCaseTests()
		{
			var agentId = Guid.NewGuid();

			var decomposer = new DecomposerStub(10, agentId);

			var taskRunRepository = new TaskRunRepository(decomposer);

			_actorRepositoryLocator = new ActorRepositoryLocator(
				new AgentRepository(taskRunRepository),
				new TaskRepository(),
				taskRunRepository,
				new AgentGatewayActorRepositoryStub());

			_tokenSource = new CancellationTokenSource();

			_actorDispatcher = new ActorDispatcher(
				_actorRepositoryLocator,
				_tokenSource.Token);

			_actorRepositoryLocator.Locate(ActorTypes.Agent).GetOrCreate(agentId);
		}

		[Fact]
		public void Audit()
		{
			var taskId = Guid.NewGuid();

			_actorDispatcher.Dispatch(new Notification(ActorTypes.Task, taskId, new CreateTask(taskId)));
			_actorDispatcher.Dispatch(new Notification(ActorTypes.Task, taskId, new RunTask(taskId)));

			const int timeoutSec = 10;

			Assert.True(WaitTaskRunFinished(taskId, timeoutSec));

			_tokenSource.Cancel();
		}

		private bool WaitTaskRunFinished(Guid taskId, int seconds)
		{
			for (var i = 0; i < seconds; i++)
			{
				Thread.Sleep(TimeSpan.FromSeconds(1));

				var task = (Task) _actorRepositoryLocator.Locate(ActorTypes.Task).GetOrCreate(taskId);
				if (!task.TaskRunId.HasValue || task.State != Task.TaskState.Idle)
				{
					continue;
				}

				var taskRun = (TaskRun) _actorRepositoryLocator.Locate(ActorTypes.TaskRun).GetOrCreate(task.TaskRunId.Value);
				if (taskRun.State == TaskRun.TaskRunState.Finished)
				{
					return true;
				}
			}

			return false;
		}
	}
}
