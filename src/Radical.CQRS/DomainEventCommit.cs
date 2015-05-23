using System;
using System.ComponentModel.DataAnnotations;

namespace Radical.CQRS
{
    public class DomainEventCommit
    {
        [Key]
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
        public DateTimeOffset PublishedOn { get; set; }
        public Guid TransactionId { get; set; }
        public string EventType { get; set; }
        public string EventBlob { get; set; }
        public int Version { get; set; }
    }
}
