using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS
{
	public interface IAggregateLoader<TAggregate> where TAggregate : class, IAggregate
	{
		TAggregate GetById( DbContext session, Guid aggregateId );

		IEnumerable<TAggregate> GetById( DbContext session, Guid[] aggregateIds );
	}
}
