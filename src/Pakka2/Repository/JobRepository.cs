using System;
using System.Collections.Generic;
using Pakka.Actor;

namespace Pakka.Repository
{
    public class JobRepository : IActorRepository
    {
        private readonly Dictionary<Guid, Job> _jobs = new Dictionary<Guid, Job>();

        public string ActorType => ActorTypes.Job;

        public IActor Create(Guid id)
        {
            var job = new Job(id);
            _jobs.Add(id, job);
            return job;
        }

        public IActor Get(Guid id)
        {
            return _jobs[id];
        }

        public void Update(IActor actor)
        {
            var job = (Job)actor;

            _jobs[job.Id] = job;
        }
    }
}
