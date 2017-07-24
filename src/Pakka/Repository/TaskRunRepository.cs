using System;
using Pakka.Actor;
using Pakka.Port;
using System.Linq;
using System.Collections.Concurrent;

namespace Pakka.Repository
{
	public class TaskRunRepository : IActorRepository, ITaskRunIdProvider
	{
		private readonly ConcurrentDictionary<Guid, TaskRun> _taskRuns = new ConcurrentDictionary<Guid, TaskRun>();
		private readonly IDecomposer _decomposer;

		public string ActorType => ActorTypes.TaskRun;

		public TaskRunRepository(IDecomposer decomposer)
		{
			_decomposer = decomposer;
		}

		public Guid GetByJobId(Guid jobId)
		{
			return _taskRuns
				.Values
				.Single(t => t.Jobs.Any(j => j.Id == jobId))
				.Id;
		}

		public IActor GetOrCreate(Guid id)
		{
			TaskRun taskRun;
			if (!_taskRuns.TryGetValue(id, out taskRun))
			{
				taskRun = new TaskRun(id, _decomposer);
				_taskRuns[id] = taskRun;
			}

			return taskRun;
		}

		public void Update(IActor actor)
		{
			var taskRun = (TaskRun) actor;

			_taskRuns[taskRun.Id] = taskRun;
		}
	}
}
