using NHibernate;
using System;
using System.Collections.Generic;

namespace Radical.CQRS
{
	public interface IAggregateLoader<TAggregate> where TAggregate : class, IAggregate
	{
		TAggregate GetById( ISession session, Guid aggregateId );

		IEnumerable<TAggregate> GetById( ISession session, Guid[] aggregateIds );
	}
}
