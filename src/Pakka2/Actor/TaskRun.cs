using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
    public class TaskRun : IActor
    {
        private List<IMessage> _messages = new List<IMessage>();
        private readonly HashSet<Guid> _jobs = new HashSet<Guid>();
        private readonly HashSet<Guid> _finishedJobs = new HashSet<Guid>();

        public Guid Id { get; }
        public Guid? TaskId { get; private set; }
        public TaskRunState State { get; private set; }

        public TaskRun(Guid id)
        {
            Id = id;
        }

        public void Execute(IMessage message)
        {
            When((dynamic)message);
        }

        private void When(Decompose message)
        {
            Console.WriteLine($"Decomposing {message.NumJobs} jobs");

            TaskId = message.TaskId;

            for (var i = 0; i < message.NumJobs; i++)
            {
                var jobId = Guid.NewGuid();

                _jobs.Add(jobId);

                _messages.Add(new CreateActor(ActorTypes.Job, jobId));
                _messages.Add(new AssignJobAgent(jobId, Id, message.AgentId));
            }

            State = TaskRunState.Waiting;

            _messages.Add(new SetTaskWaiting(TaskId.Value));

            Console.WriteLine("TaskRun decomposed");
        }

        private void When(FinishJobOnTaskRun message)
        {
            Console.WriteLine("Job finished");

            _finishedJobs.Add(message.JobId);

            if (_finishedJobs.Count == _jobs.Count)
            {
                Console.WriteLine("All jobs finished");

                State = TaskRunState.Finished;

                _messages.Add(new TaskRunFinished(TaskId.Value, Id));
            }
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

        public enum TaskRunState
        {
            Preparing,
            Waiting,
            Running,
            Stopping,
            Finished
        }
    }
}
