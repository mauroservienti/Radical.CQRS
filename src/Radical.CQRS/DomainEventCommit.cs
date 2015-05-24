using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Radical.CQRS
{
	public abstract class DomainEventCommit
	{
		[Key]
		public Guid EventId { get; set; }
		public Guid AggregateId { get; set; }
		public int Version { get; set; }

		public DateTimeOffset PublishedOn { get; set; }
		
		public Guid TransactionId { get; set; }
		
		public abstract IDomainEvent Event{ get; set; }
	}
}
