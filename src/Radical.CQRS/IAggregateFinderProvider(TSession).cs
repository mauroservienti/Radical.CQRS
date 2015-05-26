using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.CQRS
{
	public interface IAggregateFinderProvider<TSession>
	{
		IAggregateFinder<TSession, TAggregate> GetFinder<TAggregate>() where TAggregate : class, IAggregate;
		IDefaultAggregateFinder<TSession> GetDefaultFinder();
	}
}
