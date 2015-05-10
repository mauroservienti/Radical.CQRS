using Radical.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Domain.People
{
    public interface IPersonaCreata : IDomainEvent
    {

        string NuovoNome { get; set; }
    }
}
