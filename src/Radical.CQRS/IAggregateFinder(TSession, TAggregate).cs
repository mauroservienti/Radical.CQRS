using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Radical.CQRS
{
	[Contract]
	public interface IAggregateFinder<TSession, TAggregate> where TAggregate : class, IAggregate
	{
		TAggregate FindById( TSession session, Guid aggregateId );

		IEnumerable<TAggregate> FindById( TSession session, params Guid[] aggregateIds );
	}
}
