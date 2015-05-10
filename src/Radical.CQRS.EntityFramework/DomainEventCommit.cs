using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radical.CQRS
{
    public class DomainEventCommit
    {
        [Key]
        public Guid EventId { get; set; }
        public Guid AggregateId { get; set; }
        public DateTimeOffset PublishedOn { get; set; }
        public string TransactionId { get; set; }
        public string EventType { get; set; }
        public string EventBlob { get; set; }
        public int Version { get; set; }
    }
}
