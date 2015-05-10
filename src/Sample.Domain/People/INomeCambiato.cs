using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.People
{
    public interface INomeCambiato: IDomainEvent
    {
        string NuovoNome { get; set; }
    }
}
