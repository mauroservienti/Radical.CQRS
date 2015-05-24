using FluentNHibernate;
using FluentNHibernate.Mapping;
using Sample.Domain.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Domain.Data.Maps
{
	public class PersonMap : ClassMap<Person>
	{
		public PersonMap()
		{
			Table( "People" );
			Not.LazyLoad();
			Id( p => p.Id ).GeneratedBy.Assigned();
			Map( Reveal.Member<Person>( "RowVersion" ) ).OptimisticLock();
			Map( p => p.Version );
			Map( p => p.Name );
			Component( p => p.Info ).ColumnPrefix( "Info_" );
			HasMany<Address>( p => p.Addresses )
				.KeyColumn( "PersonId" )
				.Cascade.All()
				.Not.LazyLoad();
		}
	}

	class BornInfoMap : ComponentMap<BornInfo>
	{
		public BornInfoMap()
		{
			Map( b => b.Where );
			Map( b => b.When );
		}
	}

	class AddressMap : ClassMap<Address>
	{
		public AddressMap()
		{
			Table( "PersonAddresses" );
			Not.LazyLoad();
			Id( o => o.Id ).GeneratedBy.Assigned();
			Map( o => o.PersonId );
			Map( o => o.Street );
		}
	}
}
