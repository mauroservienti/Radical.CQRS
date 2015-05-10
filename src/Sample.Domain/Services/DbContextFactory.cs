using Radical.CQRS;
using Radical.CQRS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Services
{
    class DbContextFactory : IDbContextFactory
    {
        public System.Data.Entity.DbContext Create()
        {
            return new StateContext(Assembly.GetExecutingAssembly());
        }
    }
}
