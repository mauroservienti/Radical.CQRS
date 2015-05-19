using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Radical.CQRS.Data;
using Sample.Domain.People;

namespace Sample.Domain.Services
{
	public class DbContextFactory : IDbContextFactory<DomainContext>
	{
		public DomainContext Create()
		{
			var ctx = new SampleDomainContext();
			
			return ctx;
		}
	}
}
