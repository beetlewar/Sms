using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
    public class Task : IActor
    {
        private List<IMessage> _messages = new List<IMessage>();

        public Guid? CurrentTaskRunId { get; private set; }

        public Guid Id { get; }

        public TaskState State { get; private set; }

        public Task(Guid id)
        {
            Id = id;
        }

        public void Execute(IMessage message)
        {
            When((dynamic)message);
        }

        private void When(RunTask message)
        {
            Console.WriteLine("Task run");

            Guid taskRunId = Guid.NewGuid();
            CurrentTaskRunId = taskRunId;
            State = TaskState.Preparing;

            _messages.Add(new CreateActor(ActorTypes.TaskRun, taskRunId));
            _messages.Add(new Decompose(taskRunId, Id, message.AgentId, message.NumJobs));
        }

        private void When(SetTaskWaiting message)
        {
            Console.WriteLine("Task waiting");

            State = TaskState.Waiting;
        }

        private void When(TaskRunFinished message)
        {
            Console.WriteLine("TaskRun finished, Task is idle");

            State = TaskState.Idle;
        }

        private void When(object message)
        {
            throw new InvalidOperationException();
        }

        public IEnumerable<IMessage> GetMessages()
        {
            var messages = _messages;

            _messages = new List<IMessage>();

            return messages;
        }

        public enum TaskState
        {
            Idle,
            Preparing,
            Waiting,
            Running,
            Stopping
        }
    }
}
