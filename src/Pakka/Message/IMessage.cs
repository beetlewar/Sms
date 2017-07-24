using System;

namespace Pakka
{
    public interface IMessage
    {
        string ActorType { get; }
        Guid ActorId { get; }
    }
}
