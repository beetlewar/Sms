using System.Collections.Generic;

namespace Pakka.Actor
{
    public interface IActor
    {
        void Execute(IMessage message);

        IEnumerable<IMessage> GetMessages();
    }
}
