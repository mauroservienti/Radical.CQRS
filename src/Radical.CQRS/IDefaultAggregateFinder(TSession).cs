using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Radical.CQRS
{
	[Contract]
	public interface IDefaultAggregateFinder<TSession>
	{
		TAggregate FindById<TAggregate>( TSession session, Guid aggregateId ) where TAggregate : class, IAggregate;

		IEnumerable<TAggregate> FindById<TAggregate>( TSession session, params Guid[] aggregateIds ) where TAggregate : class, IAggregate;
	}
}
