using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
