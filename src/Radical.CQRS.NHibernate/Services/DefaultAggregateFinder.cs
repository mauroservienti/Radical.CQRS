using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Services
{
	class DefaultAggregateFinder : IDefaultAggregateFinder<ISession>
	{
		public TAggregate FindById<TAggregate>( ISession session, Guid aggregateId ) where TAggregate : class, IAggregate
		{
			var aggregate = session.Load<TAggregate>( aggregateId );

			return aggregate;
		}

		public IEnumerable<TAggregate> FindById<TAggregate>( ISession session, params Guid[] aggregateIds ) where TAggregate : class, IAggregate
		{
			var results = session.QueryOver<TAggregate>()
					.WhereRestrictionOn( a => a.Id )
					.IsIn( aggregateIds )
					.List();

			return results;
		}
	}
}
