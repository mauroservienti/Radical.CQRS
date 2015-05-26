using Radical.CQRS.Data;
using Sample.Domain.Companies;
using Sample.Domain.People;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Configuration;
using Topics.Radical.Helpers;

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

			modelBuilder.ComplexType<BornInfo>();

			var person = modelBuilder.Entity<Person>();
			person.HasMany( p => p.Addresses )
				.WithOptional()
				.HasForeignKey( a => a.PersonId )
				.WillCascadeOnDelete();

			modelBuilder.MapPropertiesOf<Person>(

				propertiesToSkip: new[] 
				{
					ReflectionHelper.GetPropertyName<Person>( p => p.Info ), 
					ReflectionHelper.GetPropertyName<Person>( p => p.Addresses ) 
				} );

			var companyState = modelBuilder.Entity<Company.State>()
				.ToTable( "Companies" );
			modelBuilder.MapPropertiesOf<Company.State>();
		}
	}
}
