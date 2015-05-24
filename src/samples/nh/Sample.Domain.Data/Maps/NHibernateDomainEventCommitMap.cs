using FluentNHibernate;
using FluentNHibernate.Mapping;
using Radical.CQRS;
using Sample.Domain.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Data.Maps
{
	public class NHibernateDomainEventCommitMap : ClassMap<NHibernateDomainEventCommit>
	{
		public NHibernateDomainEventCommitMap()
		{
			Table( "DomainEventCommits" );
			Not.LazyLoad();
			Id( o => o.EventId ).GeneratedBy.Assigned();
			Map( o => o.AggregateId );
			Map( o => o.Version );
			Map( o => o.TransactionId );
			Map( o => o.PublishedOn );
			Map( o => o.EventType );
			Map( o => o.EventBlob );
		}
	}
}
