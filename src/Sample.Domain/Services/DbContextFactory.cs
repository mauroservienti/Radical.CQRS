using Radical.CQRS;
using Radical.CQRS.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
