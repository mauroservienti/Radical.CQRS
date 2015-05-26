using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Services
{
	class DefaultAggregateStateFinder : IDefaultAggregateStateFinder<ISession>
	{
		public IAggregateState FindById( ISession session, Type stateType, Guid stateId )
		{
			var state = session.Load( stateType.Name, stateId );

			return ( IAggregateState )state;
		}

		public IEnumerable<IAggregateState> FindById( ISession session, Type stateType, params Guid[] stateIds )
		{
			var results = session.QueryOver<IAggregateState>( stateType.Name )
					.WhereRestrictionOn( state => state.Id )
					.IsIn( stateIds )
					.List();

			return results;
		}
	}
}
