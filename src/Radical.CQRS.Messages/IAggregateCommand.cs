using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS.Messages
{
    public interface IAggregateCommand
    {
        Guid Id { get; }
        int Version { get; }
    }
}
