using System;

namespace Pakka.Message
{
	public class CreateAgent
	{
		public Guid Id { get; }

		public CreateAgent(Guid id)
		{
			Id = id;
		}
	}
}
