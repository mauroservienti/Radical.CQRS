using FluentNHibernate.Mapping;

namespace Sample.ViewModels.Maps
{
	public class PersonViewMap : ClassMap<PersonView>
	{
		public PersonViewMap()
		{
			Table( "People" );
			Not.LazyLoad();
			Id( p => p.Id ).GeneratedBy.Assigned();
			Map( p => p.Version ).OptimisticLock();
			Map( p => p.Name );
			Component( p => p.BornInfo ).ColumnPrefix( "Info_" );
			HasMany<AddressView>( p => p.Addresses )
				.KeyColumn( "PersonId" )
				.Cascade.None()
				.Not.LazyLoad();
		}
	}

	class BornInfoViewMap : ComponentMap<BornInfoView>
	{
		public BornInfoViewMap()
		{
			Map( b => b.Where );
			Map( b => b.When );
		}
	}

	class AddressViewMap : ClassMap<AddressView>
	{
		public AddressViewMap()
		{
			Table( "PersonAddresses" );
			Not.LazyLoad();
			Id( o => o.AddressId ).Column( "Id" ).GeneratedBy.Assigned();
			Map( o => o.PersonId );
			Map( o => o.Street );
		}
	}
}
