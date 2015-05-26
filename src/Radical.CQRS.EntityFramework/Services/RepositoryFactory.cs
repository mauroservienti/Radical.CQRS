using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Radical.CQRS.Data;
using Radical.CQRS.Runtime;

namespace Radical.CQRS.Services
{
	class RepositoryFactory : IRepositoryFactory
	{
		readonly IAggregateFinderProvider<DomainContext> aggregateFinderProvider;
		readonly IAggregateStateFinderProvider<DomainContext> aggregateStateFinderProvider;
		readonly IDbContextFactory<DomainContext> _factory;

		public RepositoryFactory( IDbContextFactory<DomainContext> factory, IAggregateFinderProvider<DomainContext> aggregateFinderProvider, IAggregateStateFinderProvider<DomainContext> aggregateStateFinderProvider )
		{
			this._factory = factory;
			this.aggregateFinderProvider = aggregateFinderProvider;
			this.aggregateStateFinderProvider = aggregateStateFinderProvider;
		}

		//public IAsyncRepository OpenAsyncSession()
		//{
		//	var db = this._factory.Create();
		//	return new AsyncRepository( db, this.aggregateLoaderProvider );
		//}

		public IRepository OpenSession()
		{
			var db = this._factory.Create();
			return new SyncRepository( db, this.aggregateFinderProvider, this.aggregateStateFinderProvider );
		}
	}
}
