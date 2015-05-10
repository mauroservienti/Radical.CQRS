using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS
{
    public interface IRepositoryFactory
    {
        IRepository OpenSession();
    }
}
