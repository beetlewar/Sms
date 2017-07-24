using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
	public class Task : IActor
	{
		public Guid Id { get; }

		public Guid? TaskRunId { get; private set; }

		public TaskState State { get; private set; }

		public Task(Guid id)
		{
			Id = id;
		}

		public IEnumerable<Notification> Execute(object message)
		{
			return When((dynamic) message);
		}

		private IEnumerable<Notification> When(CreateTask message)
		{
			yield break;
		}

		private IEnumerable<Notification> When(RunTask message)
		{
			TaskRunId = message.TaskRunId;

			State = TaskState.CreatingTaskRun;

			yield return new Notification(ActorTypes.TaskRun, TaskRunId.Value, new CreateTaskRun(TaskRunId.Value, Id, message.IsAbc));
		}

		private IEnumerable<Notification> When(TaskRunCreated message)
		{
			if (message.TaskRunId != TaskRunId)
			{
				throw new InvalidOperationException();
			}

			State = TaskState.Running;

			yield break;
		}

		private IEnumerable<Notification> When(TaskRunFinished message)
		{
			if (message.TaskRunId != TaskRunId)
			{
				throw new InvalidOperationException();
			}

			State = TaskState.Idle;

			yield break;
		}

		private IEnumerable<Notification> When(object message)
		{
			throw new InvalidOperationException();
		}

		public enum TaskState
		{
			Idle,
			CreatingTaskRun,
			Running
		}
	}
}
