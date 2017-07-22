using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
    public class Job : IActor
    {
        private List<IMessage> _messages = new List<IMessage>();

        public Guid Id { get; }
        public Guid? TaskRunId { get; private set; }
        public JobState State { get; private set; }
        public Guid? AgentId { get; private set; }

        public Job(Guid id)
        {
            Id = id;
        }

        public void Execute(IMessage message)
        {
            When((dynamic)message);
        }

        private void When(object message)
        {
            throw new InvalidOperationException();
        }

        private void When(AssignJobAgent message)
        {
            Console.WriteLine("Agent assigned");

            TaskRunId = message.TaskRunId;
            AgentId = message.AgentId;
            State = JobState.Assigned;

            _messages.Add(new StartJob(message.AgentId, Id));
        }

        private void When(JobStarted message)
        {
            Console.WriteLine("Job started");

            State = JobState.Running;
        }

        private void When(JobComplete message)
        {
            Console.WriteLine("Job complete");

            State = JobState.Finished;

            _messages.Add(new FinishJobOnTaskRun(TaskRunId.Value, Id));
        }

        public IEnumerable<IMessage> GetMessages()
        {
            var messages = _messages;

            _messages = new List<IMessage>();

            return messages;
        }

        public enum JobState
        {
            Assigning,
            Assigned,
            Enqueued,
            Running,
            Finishing,
            Finished
        }
    }
}
