using System;
using System.Collections.Generic;

namespace Pakka.Port
{
	public interface IDecomposer
	{
		IEnumerable<ScanTarget> Decompose();
	}
}
