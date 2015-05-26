using Radical.CQRS.Runtime;
using NHibernate;

namespace Radical.CQRS.Services
{
	class RepositoryFactory : IRepositoryFactory
	{
		readonly IAggregateFinderProvider<ISession> aggregateFinderProvider;
		readonly IAggregateStateFinderProvider<ISession> aggregateStateFinderProvider;
		readonly ISessionFactory _factory;

		public RepositoryFactory( ISessionFactory factory, IAggregateFinderProvider<ISession> aggregateFinderProvider, IAggregateStateFinderProvider<ISession> aggregateStateFinderProvider )
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
			var session = this._factory.OpenSession();
			return new SyncRepository( session, this.aggregateFinderProvider, this.aggregateStateFinderProvider );
		}
	}
}
