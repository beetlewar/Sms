using System;
using System.Threading;
using System.Threading.Tasks;
using Pakka.Repository;

namespace Pakka
{
    public class ActorQueue
    {
        private readonly object _syncTask = new object();
        private Task _lastTask;
        private readonly IActorRepository _repository;
        private readonly CancellationToken _token;
        private readonly ActorDispatcher _actorDispatcher;

        public ActorQueue(
            ActorDispatcher actorDispatcher,
            IActorRepository repository,
            CancellationToken token)
        {
            _actorDispatcher = actorDispatcher;
            _repository = repository;
            _token = token;

            _lastTask = Task.Run(() => { });
        }

        public void Enqueue(IMessage message)
        {
            lock (_syncTask)
            {
                _lastTask = _lastTask.ContinueWith(t =>
                {
                    try
                    {
                        Execute(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }, _token);
            }
        }

        private void Execute(IMessage message)
        {
            var actor = _repository.GetOrCreate(message.ActorId);

            var messages = actor.Execute(message);

            _repository.Update(actor);

            foreach (var newMessage in messages)
            {
                _actorDispatcher.Dispatch(newMessage);
            }
        }
    }
}
