using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pakka.Port
{
    public interface ITaskIdProvider
    {
        Guid GetByJobId(Guid jobId);
    }
}
