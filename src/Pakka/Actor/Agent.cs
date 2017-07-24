using System;
using System.Collections.Generic;
using Pakka.Message;
using Pakka.Port;

namespace Pakka.Actor
{
	public class Agent : IActor
	{
		public Guid Id { get; }

		private readonly ITaskRunIdProvider _taskIdProvider;

		public Agent(Guid id, ITaskRunIdProvider taskIdProvider)
		{
			Id = id;
			_taskIdProvider = taskIdProvider;
		}

		public IEnumerable<Notification> Execute(object message)
		{
			return When((dynamic) message);
		}

		private IEnumerable<Notification> When(object message)
		{
			throw new InvalidOperationException();
		}

		private IEnumerable<Notification> When(CreateAgent message)
		{
			yield break;
		}

		private IEnumerable<Notification> When(StartJob message)
		{
			yield return new Notification(ActorTypes.AgentJobGateway, message.JobId, message);
		}

		private IEnumerable<Notification> When(JobEnqueued message)
		{
			var taskId = _taskIdProvider.GetByJobId(message.Id);

			yield return new Notification(ActorTypes.TaskRun, taskId, message);
		}

		private IEnumerable<Notification> When(JobStarted message)
		{
			var taskId = _taskIdProvider.GetByJobId(message.Id);

			yield return new Notification(ActorTypes.TaskRun, taskId, message);
		}

		private IEnumerable<Notification> When(JobFinished message)
		{
			var taskId = _taskIdProvider.GetByJobId(message.Id);

			yield return new Notification(ActorTypes.TaskRun, taskId, message);
		}

		private IEnumerable<Notification> When(JobResult message)
		{
			var taskId = _taskIdProvider.GetByJobId(message.Id);

			yield return new Notification(ActorTypes.TaskRun, taskId, message);
		}
	}
}
