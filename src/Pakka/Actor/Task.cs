using System;
using System.Collections.Generic;
using Pakka.Message;
using Pakka.Port;

namespace Pakka.Actor
{
    public class Task : IActor
    {
        private readonly IDecomposer _decomposer;

        public TaskRun CurrentTaskRun { get; private set;}

        public Guid Id { get; }

        public Task(Guid id, IDecomposer decomposer)
        {
            Id = id;
            _decomposer = decomposer;
        }

        public IEnumerable<IMessage> Execute(IMessage message)
        {
            return When((dynamic)message);
        }

        private IEnumerable<IMessage> When(CreateTask message)
        {
            yield break;
        }

        private IEnumerable<IMessage> When(RunTask message)
        {
            Console.WriteLine("Running task");

            var taskRun = new TaskRun(Guid.NewGuid(), Id, _decomposer);
            CurrentTaskRun = taskRun;

            Console.WriteLine("Task run");

            yield return new Decompose(Id);
        }

        private IEnumerable<IMessage> When(Decompose message)
        {
            Console.WriteLine("Decomposing");

            var messages = CurrentTaskRun.Decompose();

            Console.WriteLine("Decomposed");

            return messages;
        }

        private IEnumerable<IMessage> When(StartJobs message)
        {
            Console.WriteLine("Starting jobs");

            var messages = CurrentTaskRun.StartJobs();

            Console.WriteLine("Jobs started");

            return messages;
        }

        private IEnumerable<IMessage> When(JobEnqueued message)
        {
            Console.WriteLine("Job enqueued");

            CurrentTaskRun.JobEnqueued(message.JobId);

            yield break;
        }

        private IEnumerable<IMessage> When(JobStarted message)
        {
            Console.WriteLine("Job started");

            CurrentTaskRun.JobStarted(message.JobId);

            yield break;
        }

        private IEnumerable<IMessage> When(JobFinished message)
        {
            Console.WriteLine("Job finisihed");

            CurrentTaskRun.JobFinished(message.JobId);

            yield break;
        }

        private IEnumerable<IMessage> When(object message)
        {
            throw new InvalidOperationException();
        }
    }
}
