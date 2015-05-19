using Radical.CQRS.Data;
using Sample.Domain.People;
using System.Data.Entity;

namespace Sample.Domain
{
	public class SampleDomainContext : DomainContext
	{
		protected override void OnModelCreating( DbModelBuilder modelBuilder )
		{
			base.OnModelCreating( modelBuilder );

			modelBuilder.Entity<Address>()
					.ToTable( "dbo.PersonAddresses" );
			modelBuilder.MapPropertiesOf<Address>();

			modelBuilder.Entity<Person>()
				.HasMany( p => p.Addresses )
				.WithOptional()
				.HasForeignKey( a => a.PersonId )
				.WillCascadeOnDelete();

			modelBuilder.MapPropertiesOf<Person>( pi => pi.Name != "Addresses" );
		}
	}
}
