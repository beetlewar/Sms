using System;
using System.Threading;
using Pakka;
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
        private readonly Guid _agentId;

        public UseCaseTests()
        {
            _agentId = Guid.NewGuid();

            var decomposer = new DecomposerStub(10, _agentId);

            var taskRepository = new TaskRepository(decomposer);

            _actorRepositoryLocator = new ActorRepositoryLocator(
                new AgentRepository(taskRepository),
                taskRepository,
                new AgentGatewayActorRepositoryStub());

            _tokenSource = new CancellationTokenSource();

            _actorDispatcher = new ActorDispatcher(
                _actorRepositoryLocator,
                _tokenSource.Token);

            _actorRepositoryLocator.Locate(ActorTypes.Agent).GetOrCreate(_agentId);
        }

        [Fact]
        public void Audit()
        {
            var taskId = Guid.NewGuid();

            _actorDispatcher.Dispatch(new CreateTask(taskId));
            _actorDispatcher.Dispatch(new RunTask(taskId));

            const int timeoutSec = 10;

            Assert.True(WaitTaskRunFinished(taskId, timeoutSec));

            _tokenSource.Cancel();
        }

        private bool WaitTaskRunFinished(Guid taskId, int seconds)
        {
            for (int i = 0; i < seconds; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                var task = (Task)_actorRepositoryLocator.Locate(ActorTypes.Task).GetOrCreate(taskId);
                if (task.CurrentTaskRun != null && task.CurrentTaskRun.State == TaskRun.TaskRunState.Finished)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
