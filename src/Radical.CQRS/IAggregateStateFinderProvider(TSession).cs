using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radical.CQRS
{
	public interface IAggregateStateFinderProvider<TSession>
	{
		IAggregateStateFinder<TSession> GetFinder( Type stateType );
		IDefaultAggregateStateFinder<TSession> GetDefaultFinder();
	}
}
