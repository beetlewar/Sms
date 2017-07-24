using Pakka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Port
{
    public interface IDecomposer
    {
        IEnumerable<Job> Decompose();
    }
}
