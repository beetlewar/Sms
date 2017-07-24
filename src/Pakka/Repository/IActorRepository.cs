using System;
using Pakka.Actor;

namespace Pakka.Repository
{
    public interface IActorRepository
    {
        string ActorType { get; }

        IActor GetOrCreate(Guid id);

        void Update(IActor actor);
    }
}
