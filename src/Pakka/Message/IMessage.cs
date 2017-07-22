using System;

namespace Pakka
{
    public interface IMessage
    {
        Guid ActorId { get; }
    }
}
