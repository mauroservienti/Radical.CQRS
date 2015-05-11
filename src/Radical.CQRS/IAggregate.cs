using System;
using System.Collections.Generic;

namespace Radical.CQRS
{
	public interface IAggregate
	{
		Guid Id { get; }
		int Version { get; }

		Boolean IsChanged { get; }
		IEnumerable<IDomainEvent> GetUncommittedEvents();
		void ClearUncommittedEvents();
	}
}
