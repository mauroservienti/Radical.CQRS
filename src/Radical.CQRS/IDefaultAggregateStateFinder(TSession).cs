using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Topics.Radical.ComponentModel;

namespace Radical.CQRS
{
	public interface IDefaultAggregateStateFinder<TSession>
	{
		IAggregateState FindById( TSession session, Type stateType, AggregateQuery aggregateQuery);

		IEnumerable<IAggregateState> FindById( TSession session, Type stateType, params AggregateQuery[] aggregateQueries);
	}
}
