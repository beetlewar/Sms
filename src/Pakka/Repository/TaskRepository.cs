using System;
using System.Collections.Generic;
using Pakka.Actor;
using Pakka.Port;
using System.Linq;
using System.Collections.Concurrent;

namespace Pakka.Repository
{
    public class TaskRepository : IActorRepository, ITaskIdProvider
    {
        private readonly ConcurrentDictionary<Guid, Task> _tasks = new ConcurrentDictionary<Guid, Task>();
        private readonly IDecomposer _decomposer;

        public string ActorType => ActorTypes.Task;

        public TaskRepository(IDecomposer decomposer)
        {
            _decomposer = decomposer;
        }

        public Guid GetByJobId(Guid jobId)
        {
            return _tasks
                .Values
                .Single(t => t.CurrentTaskRun != null && t.CurrentTaskRun.Jobs.Any(j => j.Id == jobId))
                .Id;
        }

        public IActor GetOrCreate(Guid id)
        {
            Task task;
            if (!_tasks.TryGetValue(id, out task))
            {
                task = new Task(id, _decomposer);
                _tasks[id] = task;
            }

            return task;
        }

        public void Update(IActor actor)
        {
            var task = (Task)actor;

            _tasks[task.Id] = task;
        }
    }
}
