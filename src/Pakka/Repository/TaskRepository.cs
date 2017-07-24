using System;
using Pakka.Actor;
using System.Collections.Concurrent;

namespace Pakka.Repository
{
	public class TaskRepository : IActorRepository
	{
		private readonly ConcurrentDictionary<Guid, Task> _tasks = new ConcurrentDictionary<Guid, Task>();

		public IActor GetOrCreate(Guid id)
		{
			Task task;
			if (!_tasks.TryGetValue(id, out task))
			{
				task = new Task(id);
				_tasks[id] = task;
			}

			return task;
		}

		public void Update(IActor actor)
		{
			var task = (Task) actor;

			_tasks[task.Id] = task;
		}
	}
}
