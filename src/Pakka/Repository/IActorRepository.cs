using System;
using Pakka.Actor;

namespace Pakka.Repository
{
	public interface IActorRepository
	{
		IActor GetOrCreate(Guid id);

		void Update(IActor actor);
	}
}
