using Radical.CQRS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Services
{
	class DefaultAggregateStateFinder : IDefaultAggregateStateFinder<DomainContext>
	{
		public IAggregateState FindById( DomainContext session, Type stateType, Guid stateId )
		{
			var db = session.Set( stateType );
			var state = db.Find( stateId );

			return ( IAggregateState )state;
		}

		public IEnumerable<IAggregateState> FindById( DomainContext session, Type stateType, params Guid[] stateIds )
		{
			var db = session.Set( stateType ).Cast<IAggregateState>();
			var results = db.Where( state => stateIds.Contains( state.Id ) ).ToList();

			return results;
		}
	}
}
