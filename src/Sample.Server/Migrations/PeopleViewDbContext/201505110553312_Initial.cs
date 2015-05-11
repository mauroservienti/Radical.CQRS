using System.Data.Entity.Migrations;
using Radical.CQRS.DbMigrations;

namespace Sample.Server.Migrations.PeopleViewDbContext
{
	public partial class Initial : DbMigration
	{
		public override void Up()
		{
			this.CreateView( "dbo.PeopleView", "select * from dbo.People" );

		}

		public override void Down()
		{
			this.DropView( "dbo.PeopleView" );
		}
	}
}
