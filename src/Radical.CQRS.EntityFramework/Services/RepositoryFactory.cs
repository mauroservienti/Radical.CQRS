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

        public IRepository OpenAsyncSession()
        {
            var db = this._factory.Create();
            return new Repository(db);
        }
    }
}
