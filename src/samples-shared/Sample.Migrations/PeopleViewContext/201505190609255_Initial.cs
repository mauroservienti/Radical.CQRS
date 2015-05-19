namespace Sample.Migrations.PeopleViewContext
{
    using System;
    using System.Data.Entity.Migrations;
	using Radical.CQRS.DbMigrations;
    
    public partial class Initial : DbMigration
    {
		public override void Up()
		{
			this.CreateView( "dbo.PeopleView", "SELECT dbo.People.* FROM dbo.People" );

		}

		public override void Down()
		{
			this.DropView( "dbo.PeopleView" );
		}
    }
}
