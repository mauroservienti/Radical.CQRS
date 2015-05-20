using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Radical.CQRS.Data;
using Radical.CQRS.Runtime;

namespace Radical.CQRS.Services
{
	class RepositoryFactory : IRepositoryFactory
	{
		readonly AggregateLoaderProvider aggregateLoaderProvider;
		readonly IDbContextFactory<DomainContext> _factory;

		public RepositoryFactory( IDbContextFactory<DomainContext> factory, AggregateLoaderProvider aggregateLoaderProvider )
		{
			this._factory = factory;
			this.aggregateLoaderProvider = aggregateLoaderProvider;
		}

		//public IAsyncRepository OpenAsyncSession()
		//{
		//	var db = this._factory.Create();
		//	return new AsyncRepository( db, this.aggregateLoaderProvider );
		//}

		public IRepository OpenSession()
		{
			var db = this._factory.Create();
			return new SyncRepository( db, this.aggregateLoaderProvider );
		}
	}
}
