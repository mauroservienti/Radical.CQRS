using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Reflection;
using Radical.CQRS.DbMigrations;

namespace Sample.Server.Migrations.DomainContext
{
	internal sealed class Configuration : DbMigrationsConfiguration<Radical.CQRS.Data.DomainContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			SetSqlGenerator( "System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator() );
		}

		protected override void Seed( Radical.CQRS.Data.DomainContext context )
		{
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data. E.g.
			//
			//    context.People.AddOrUpdate(
			//      p => p.FullName,
			//      new Person { FullName = "Andrew Peters" },
			//      new Person { FullName = "Brice Lambson" },
			//      new Person { FullName = "Rowan Miller" }
			//    );
			//
		}
	}
}
