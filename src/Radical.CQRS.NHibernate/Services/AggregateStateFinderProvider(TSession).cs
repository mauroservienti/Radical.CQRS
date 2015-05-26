using NHibernate;
using System;
using Topics.Radical;

namespace Radical.CQRS.Services
{
	class AggregateStateFinderProvider : IAggregateStateFinderProvider<ISession>
	{
		readonly IServiceProvider container;

		public AggregateStateFinderProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IAggregateStateFinder<ISession> GetFinder( Type stateType )
		{
			var type = typeof( IAggregateStateFinder<,> ).MakeGenericType( new[] { typeof( ISession ), stateType } );

			return ( IAggregateStateFinder<ISession> )this.container.GetService( type );
		}

		public IDefaultAggregateStateFinder<ISession> GetDefaultFinder()
		{
			return this.container.GetService<IDefaultAggregateStateFinder<ISession>>();
		}
	}
}
