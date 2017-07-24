using System;
using System.Collections.Generic;

namespace Pakka.Port
{
	public interface IDecomposer
	{
		/// <summary>
		/// Возвращает список идентификаторов агентов, на которых назначены джобы
		/// </summary>
		/// <returns></returns>
		IEnumerable<Guid> Decompose();
	}
}
