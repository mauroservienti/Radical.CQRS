using Radical.CQRS.Data;
using System;
using Topics.Radical;

namespace Radical.CQRS.Services
{
	class AggregateStateFinderProvider : IAggregateStateFinderProvider<DomainContext>
	{
		readonly IServiceProvider container;

		public AggregateStateFinderProvider( IServiceProvider container )
		{
			this.container = container;
		}

		public IAggregateStateFinder<DomainContext> GetFinder( Type stateType )
		{
			var type = typeof( IAggregateStateFinder<,> ).MakeGenericType( new[] { typeof( DomainContext ), stateType } );

			return ( IAggregateStateFinder<DomainContext> )this.container.GetService( type );
		}

		public IDefaultAggregateStateFinder<DomainContext> GetDefaultFinder()
		{
			return this.container.GetService<IDefaultAggregateStateFinder<DomainContext>>();
		}
	}
}
