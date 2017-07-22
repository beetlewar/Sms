using System;
using System.Threading;
using Pakka;
using Pakka.Actor;
using Pakka.Message;
using Pakka.Repository;
using Xunit;

namespace PakkaTests
{
    public class UseCaseTests
    {
        private readonly ActorRepositoryLocator _actorRepositoryLocator;
        private readonly CancellationTokenSource _tokenSource;
        private readonly ActorDispatcher _actorDispatcher;

        public UseCaseTests()
        {
            _actorRepositoryLocator = ActorRepositoryLocator.All();

            _tokenSource = new CancellationTokenSource();

            _actorDispatcher = new ActorDispatcher(
                _actorRepositoryLocator,
                _tokenSource.Token);
        }

        [Fact]
        public void Audit()
        {
            var taskId = Guid.NewGuid();
            var agentId = Guid.NewGuid();

            _actorDispatcher.Dispatch(new CreateActor(ActorTypes.Agent, agentId));
            _actorDispatcher.Dispatch(new CreateActor(ActorTypes.Task, taskId));
            _actorDispatcher.Dispatch(new RunTask(taskId, agentId, 100000));

            const int timeoutSec = 60;

            var taskRunId = WaitTaskRunCreated(taskId, timeoutSec);
            Assert.True(taskRunId.HasValue);
            Assert.True(WaitTaskRunFinished(taskRunId.Value, timeoutSec));
            Assert.True(WaitTaskIdle(taskId, timeoutSec));

            _tokenSource.Cancel();
        }

        private Guid? WaitTaskRunCreated(Guid taskId, int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var task = (Task)_actorRepositoryLocator.Locate(ActorTypes.Task).Get(taskId);
                if (task.CurrentTaskRunId.HasValue)
                {
                    return task.CurrentTaskRunId;
                }
            }

            return null;
        }

        private bool WaitTaskRunFinished(Guid taskRunId, int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var taskRun = (TaskRun)_actorRepositoryLocator.Locate(ActorTypes.TaskRun).Get(taskRunId);
                if (taskRun.State == TaskRun.TaskRunState.Finished)
                {
                    return true;
                }
            }

            return false;
        }

        private bool WaitTaskIdle(Guid taskId, int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var task = (Task)_actorRepositoryLocator.Locate(ActorTypes.Task).Get(taskId);
                if (task.State == Task.TaskState.Idle)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
