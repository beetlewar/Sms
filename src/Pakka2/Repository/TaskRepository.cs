using System;
using System.Collections.Generic;
using Pakka.Actor;

namespace Pakka.Repository
{
    public class TaskRepository : IActorRepository
    {
        private readonly Dictionary<Guid, Task> _tasks = new Dictionary<Guid, Task>();

        public string ActorType => ActorTypes.Task;

        public IActor Create(Guid id)
        {
            var task = new Task(id);
            _tasks.Add(id, task);
            return task;
        }

        public IActor Get(Guid id)
        {
            return _tasks[id];
        }

        public void Update(IActor actor)
        {
            var task = (Task)actor;

            _tasks[task.Id] = task;
        }
    }
}
