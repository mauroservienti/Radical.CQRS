using Radical.CQRS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Services
{
	class DefaultAggregateFinder : IDefaultAggregateFinder<DomainContext>
	{
		public TAggregate FindById<TAggregate>( DomainContext session, Guid aggregateId ) where TAggregate : class, IAggregate
		{
			var db = session.Set<TAggregate>();
			var aggregate = db.Find( aggregateId );

			return aggregate;
		}

		public IEnumerable<TAggregate> FindById<TAggregate>( DomainContext session, params Guid[] aggregateIds ) where TAggregate : class, IAggregate
		{
			var db = session.Set<TAggregate>();
			var results = db.Where( a => aggregateIds.Contains( a.Id ) )
				.ToList();

			return results;
		}
	}
}
