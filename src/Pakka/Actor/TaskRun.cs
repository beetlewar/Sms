using System;
using System.Collections.Generic;
using Pakka.Message;
using System.Linq;
using Pakka.Port;

namespace Pakka.Actor
{
    public class TaskRun
    {
        private readonly List<Job> _jobs = new List<Job>();
        private readonly IDecomposer _decomposer;

        public Guid Id { get; }
        public Guid TaskId { get; }
        public TaskRunState State { get; private set; }

        public IEnumerable<Job> Jobs => _jobs;

        public TaskRun(Guid id, Guid taskId, IDecomposer decomposer)
        {
            Id = id;
            TaskId = taskId;
            _decomposer = decomposer;
        }

        public IEnumerable<IMessage> Decompose()
        {
            foreach(var job in _decomposer.Decompose())
            {
                _jobs.Add(job);
            }

            State = TaskRunState.Waiting;

            yield return new StartJobs(TaskId);
        }

        public IEnumerable<IMessage> StartJobs()
        {
            foreach(var job in _jobs)
            {
                yield return job.StartJob();
            }
        }

        public void JobEnqueued(Guid id)
        {
            var job = _jobs.Single(j => j.Id == id);
            job.Enqueued();
        }

        public void JobStarted(Guid id)
        {
            var job = _jobs.Single(j => j.Id == id);
            job.Started();

            State = TaskRunState.Running;
        }

        public void JobFinished(Guid id)
        {
            var job = _jobs.Single(j => j.Id == id);
            job.Finished();

            if (_jobs.All(j => j.State == Job.JobState.Finished))
            {
                State = TaskRunState.Finished;
            }
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
