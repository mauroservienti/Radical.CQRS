using System.Data.Entity.Infrastructure;
using Radical.CQRS.Data;
using Radical.CQRS.Runtime;

namespace Radical.CQRS.Services
{
    class RepositoryFactory : IRepositoryFactory
    {
        readonly IDbContextFactory<DomainContext> _factory;

		public RepositoryFactory( IDbContextFactory<DomainContext> factory )
        {
            this._factory = factory;
        }

        public IAsyncRepository OpenAsyncSession()
        {
            var db = this._factory.Create();
            return new AsyncRepository(db);
        }

		public IRepository OpenSession()
		{
			var db = this._factory.Create();
			return new SyncRepository( db );
		}
    }
}
