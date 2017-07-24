using System;
using System.Collections.Generic;
using Pakka.Message;
using Pakka.Port;

namespace Pakka.Actor
{
    public class Agent : IActor
    {
        public Guid Id { get; }

        private readonly ITaskIdProvider _taskIdProvider;

        public Agent(Guid id, ITaskIdProvider taskIdProvider)
        {
            Id = id;
            _taskIdProvider = taskIdProvider;
        }

        public IEnumerable<IMessage> Execute(IMessage message)
        {
            return When((dynamic)message);
        }

        private IEnumerable<IMessage> When(object message)
        {
            throw new InvalidOperationException();
        }

        private IEnumerable<IMessage> When(CreateAgent message)
        {
            Console.WriteLine("Agent created");

            yield break;
        }

        private IEnumerable<IMessage> When(StartJob message)
        {
            Console.WriteLine("Starting job");

            yield return new GatewayStartJob(message.JobId, Id);
        }

        private IEnumerable<IMessage> When(GatewayJobEnqueued message)
        {
            Console.WriteLine("Job enqueued");

            var taskId = _taskIdProvider.GetByJobId(message.JobId);

            yield return new JobEnqueued(taskId, message.JobId);
        }

        private IEnumerable<IMessage> When(GatewayJobStarted message)
        {
            Console.WriteLine("Job started");

            var taskId = _taskIdProvider.GetByJobId(message.JobId);

            yield return new JobStarted(taskId, message.JobId);
        }

        private IEnumerable<IMessage> When(GatewayJobFinished message)
        {
            Console.WriteLine("Job finished");

            var taskId = _taskIdProvider.GetByJobId(message.JobId);

            yield return new JobFinished(taskId, message.JobId);
        }
    }
}
