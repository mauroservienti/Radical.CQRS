using System;

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
