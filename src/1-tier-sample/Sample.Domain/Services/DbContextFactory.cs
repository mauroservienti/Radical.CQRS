using System.Data.Entity.Infrastructure;
using System.Reflection;
using Radical.CQRS.Data;

namespace Sample.Domain.Services
{
	public class DbContextFactory : IDbContextFactory<DomainContext>
    {
		public DomainContext Create()
        {
            return new DomainContext(Assembly.GetExecutingAssembly());
        }
    }
}
