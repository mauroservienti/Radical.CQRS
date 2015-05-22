using Radical.CQRS.Runtime;
using NHibernate;

namespace Radical.CQRS.Services
{
	class RepositoryFactory : IRepositoryFactory
	{
		readonly AggregateLoaderProvider aggregateLoaderProvider;
		readonly ISessionFactory _factory;

		public RepositoryFactory( ISessionFactory factory, AggregateLoaderProvider aggregateLoaderProvider )
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
			var session = this._factory.OpenSession();
			return new SyncRepository( session, this.aggregateLoaderProvider );
		}
	}
}
