using System;
using System.Collections.Generic;
using System.Linq;
using Pakka.Message;
using Pakka.Port;

namespace Pakka.Actor
{
	public class TaskRun : IActor
	{
		private Guid? _abcHdJobId;
		private readonly Queue<Job> _pendingJobs = new Queue<Job>();
		private readonly List<Job> _jobs = new List<Job>();
		private readonly IDecomposer _decomposer;
		private bool _isAbc;

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
			_isAbc = message.IsAbc;

			TaskId = message.TaskId;

			var decomposeResult = _decomposer.Decompose().ToArray();

			if (_isAbc && decomposeResult.Length != 1)
			{
				throw new InvalidOperationException("ABC must be decomposed into 1 job");
			}

			foreach (var scanTarget in decomposeResult)
			{
				var job = CreateJob(scanTarget);

				if (_isAbc)
				{
					_abcHdJobId = job.Id;
				}

				_pendingJobs.Enqueue(job);
			}

			State = TaskRunState.Waiting;

			yield return new Notification(ActorTypes.Task, TaskId, new TaskRunCreated(Id));
			yield return new Notification(ActorTypes.TaskRun, Id, new StartJobs());
		}

		private static Job CreateJob(ScanTarget scanTarget)
		{
			var job = new Job(Guid.NewGuid(), scanTarget.AgentId, scanTarget.Target);
			return job;
		}

		private IEnumerable<Notification> When(StartJobs message)
		{
			while (_pendingJobs.Count > 0)
			{
				var pendingJob = _pendingJobs.Dequeue();
				_jobs.Add(pendingJob);

				yield return pendingJob.StartJob();
			}
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

		public IEnumerable<Notification> When(JobResult message)
		{
			if (!_isAbc)
			{
				yield break;
			}

			if (message.Id != _abcHdJobId)
			{
				yield break;
			}

			foreach (var messageTarget in message.Targets)
			{
				_pendingJobs.Enqueue(CreateJob(messageTarget));
			}

			yield return new Notification(ActorTypes.TaskRun, Id, new StartJobs());
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
