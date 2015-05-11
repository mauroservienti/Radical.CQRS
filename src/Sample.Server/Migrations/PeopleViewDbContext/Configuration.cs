using System.Data.Entity.Migrations;
using Radical.CQRS.DbMigrations;

namespace Sample.Server.Migrations.PeopleViewDbContext
{
	internal sealed class Configuration : DbMigrationsConfiguration<Sample.ViewModels.PeopleViewDbContext>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = false;
			SetSqlGenerator( "System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator() );
		}

		protected override void Seed(Sample.ViewModels.PeopleViewDbContext context)
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
