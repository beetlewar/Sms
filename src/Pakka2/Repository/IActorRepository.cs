using System;
using Pakka.Actor;

namespace Pakka.Repository
{
    public interface IActorRepository
    {
        string ActorType { get; }

        IActor Create(Guid id);

        IActor Get(Guid id);

        void Update(IActor actor);
    }
}
