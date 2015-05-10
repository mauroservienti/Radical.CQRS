using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS
{
	public interface IDomainEvent
	{
        Guid Id { get; set; }
        Guid AggregateId { get; set; }
        Int32 AggregateVersion { get; set; }
        DateTimeOffset OccurredAt { get; set; }
	}
}
