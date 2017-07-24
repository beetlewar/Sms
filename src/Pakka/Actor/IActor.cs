using System.Collections.Generic;

namespace Pakka.Actor
{
    public interface IActor
    {
        IEnumerable<IMessage> Execute(IMessage message);
    }
}
