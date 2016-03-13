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
		TAggregate FindById<TAggregate>( TSession session, AggregateQuery aggregateQuery ) where TAggregate : class, IAggregate;

		IEnumerable<TAggregate> FindById<TAggregate>( TSession session, params AggregateQuery[] aggregateQueries ) where TAggregate : class, IAggregate;
	}
}
