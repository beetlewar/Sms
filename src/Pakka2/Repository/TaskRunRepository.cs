using System;
using System.Collections.Generic;
using Pakka.Actor;

namespace Pakka.Repository
{
    public class TaskRunRepository : IActorRepository
    {
        private readonly Dictionary<Guid, TaskRun> _taskRuns = new Dictionary<Guid, TaskRun>();

        public string ActorType => ActorTypes.TaskRun;

        public IActor Create(Guid id)
        {
            var taskRun = new TaskRun(id);

            _taskRuns.Add(id, taskRun);

            return taskRun;
        }

        public IActor Get(Guid id)
        {
            return _taskRuns[id];
        }

        public void Update(IActor actor)
        {
            var taskRun = (TaskRun)actor;

            _taskRuns[taskRun.Id] = taskRun;
        }
    }
}
