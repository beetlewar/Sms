using System;
using System.Collections.Generic;
using System.Linq;
using Pakka.Message;
using Pakka.Port;

namespace Pakka.Actor
{
	public class TaskRun : IActor
	{
		private readonly List<Job> _jobs = new List<Job>();
		private readonly IDecomposer _decomposer;

		public Guid Id { get; }

		public Guid TaskId { get; private set; }

		public TaskRunState State { get; private set; }

		public IEnumerable<Job> Jobs => _jobs;

		public TaskRun(Guid id, IDecomposer decomposer)
		{
			Id = id;
			_decomposer = decomposer;
		}

		public IEnumerable<Notification> Execute(object message)
		{
			return When((dynamic) message);
		}

		private IEnumerable<Notification> When(CreateTaskRun message)
		{
			TaskId = message.TaskId;

			foreach (var agentId in _decomposer.Decompose())
			{
				var job = new Job(Guid.NewGuid(), agentId);
				_jobs.Add(job);
			}

			State = TaskRunState.Waiting;

			yield return new Notification(ActorTypes.Task, TaskId, new TaskRunCreated(Id));
			yield return new Notification(ActorTypes.TaskRun, Id, new StartJobs());
		}

		private IEnumerable<Notification> When(StartJobs message)
		{
			return _jobs.Select(j => j.StartJob());
		}

		private IEnumerable<Notification> When(JobEnqueued message)
		{
			var job = _jobs.Single(j => j.Id == message.Id);
			job.Enqueued();

			yield break;
		}

		public IEnumerable<Notification> When(JobStarted message)
		{
			var job = _jobs.Single(j => j.Id == message.Id);
			job.Started();

			State = TaskRunState.Running;

			yield break;
		}

		public IEnumerable<Notification> When(JobFinished message)
		{
			var job = _jobs.Single(j => j.Id == message.Id);
			job.Finished();

			if (_jobs.All(j => j.State == Job.JobState.Finished))
			{
				State = TaskRunState.Finished;
				yield return new Notification(ActorTypes.Task, TaskId, new TaskRunFinished(Id));
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
