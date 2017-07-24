using System;
using System.Collections.Concurrent;
using System.Threading;
using Pakka.Message;
using Pakka.Repository;

namespace Pakka
{
    public class ActorDispatcher
    {
        private readonly ConcurrentDictionary<Tuple<string, Guid>, ActorQueue> _actorQueues;
        private readonly ActorRepositoryLocator _actoryRepositoryLocator;
        private readonly CancellationToken _token;

        public ActorDispatcher(ActorRepositoryLocator actoryRepositoryLocator, CancellationToken token)
        {
            _actorQueues = new ConcurrentDictionary<Tuple<string, Guid>, ActorQueue>();
            _actoryRepositoryLocator = actoryRepositoryLocator;
            _token = token;
        }

        public void Dispatch(IMessage message)
        {
            ActorQueue actorQueue;
            var key = new Tuple<string, Guid>(message.ActorType, message.ActorId);
            if (!_actorQueues.TryGetValue(key, out actorQueue))
            {
                var repository = _actoryRepositoryLocator.Locate(message.ActorType);
                actorQueue = new ActorQueue(this, repository, _token);
                _actorQueues[key] = actorQueue;
            }

            actorQueue.Enqueue(message);
        }
    }
}
