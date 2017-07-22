using System;
using System.Collections.Generic;
using Pakka.Message;

namespace Pakka.Actor
{
    public class Agent : IActor
    {
        private List<IMessage> _messages = new List<IMessage>();

        public Guid Id { get; }

        public Agent(Guid id)
        {
            Id = id;
        }

        public void Execute(IMessage message)
        {
            When((dynamic)message);
        }

        private void When(object message)
        {
            throw new InvalidOperationException();
        }

        private void When(StartJob message)
        {
            Console.WriteLine("Starting job");

            _messages.Add(new JobStarted(message.JobId));

            Console.WriteLine("Completing job");

            _messages.Add(new JobComplete(message.JobId));
        }

        public IEnumerable<IMessage> GetMessages()
        {
            var messages = _messages;

            _messages = new List<IMessage>();

            return messages;
        }
    }
}
