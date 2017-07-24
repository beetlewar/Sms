using System;

namespace Pakka.Message
{
    public class JobEnqueued
    {
        public Guid Id { get; }

        public JobEnqueued(Guid id)
        {
            Id = id;
        }
    }
}
