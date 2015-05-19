using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Sample.ViewModels
{
	class PeopleViewContext : DbContext, IPeopleViewContext
	{
		readonly DbSet<PersonView> _personViewSet;

		public PeopleViewContext()
		{
			this._personViewSet = this.Set<PersonView>();
		}

		public IQueryable<PersonView> PeopleView
		{
			get { return this._personViewSet.AsNoTracking(); }
		}

		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			
			modelBuilder.ComplexType<BornInfoView>();

			var address = modelBuilder.Entity<AddressView>();
			address.ToTable( "dbo.PersonAddresses" );
			address.Property( a => a.AddressId ).HasColumnName( "Id" );
			address.HasKey( a => a.AddressId );

			var person = modelBuilder.Entity<PersonView>();
			person.ToTable( "dbo.People" );

			person.HasMany( p => p.Addresses )
				.WithOptional()
				.HasForeignKey( a => a.PersonId );

			person.Property( p => p.BornInfo.When ).HasColumnName( "Info_When" );
			person.Property( p => p.BornInfo.Where ).HasColumnName( "Info_Where" );
		}
	}
}
