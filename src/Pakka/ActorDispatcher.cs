using System;
using System.Collections.Concurrent;
using System.Threading;
using Pakka.Message;
using Pakka.Repository;

namespace Pakka
{
    public class ActorDispatcher
    {
        private readonly ConcurrentDictionary<Guid, ActorQueue> _actorQueues;
        private readonly ActorRepositoryLocator _actoryRepositoryLocator;
        private readonly CancellationToken _token;

        public ActorDispatcher(ActorRepositoryLocator actoryRepositoryLocator, CancellationToken token)
        {
            _actorQueues = new ConcurrentDictionary<Guid, ActorQueue>();
            _actoryRepositoryLocator = actoryRepositoryLocator;
            _token = token;
        }

        public void Dispatch(IMessage message)
        {
            When((dynamic)message);
        }

        private void When(CreateActor message)
        {
            var repository = _actoryRepositoryLocator.Locate(message.ActorType);

            var actorQueue = new ActorQueue(this, repository, _token);

            _actorQueues[message.ActorId] = actorQueue;

            repository.Create(message.ActorId);
        }

        private void When(IMessage message)
        {
            _actorQueues[message.ActorId].Enqueue(message);
        }
    }
}
