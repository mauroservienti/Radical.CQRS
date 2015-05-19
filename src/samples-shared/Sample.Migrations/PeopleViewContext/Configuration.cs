namespace Sample.Migrations.PeopleViewContext
{
	using Radical.CQRS.DbMigrations;
	using System;
	using System.Data.Entity;
	using System.Data.Entity.Migrations;
	using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sample.ViewModels.PeopleViewContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"PeopleViewContext";
			SetSqlGenerator( "System.Data.SqlClient", new CustomSqlServerMigrationSqlGenerator() );
        }

        protected override void Seed(Sample.ViewModels.PeopleViewContext context)
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
