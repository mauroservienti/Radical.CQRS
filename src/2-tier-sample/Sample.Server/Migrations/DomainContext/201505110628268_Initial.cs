using System.Data.Entity.Migrations;

namespace Sample.Server.Migrations.DomainContext
{
	public partial class Initial : DbMigration
	{
		public override void Up()
		{
			CreateTable(
			   "dbo.DomainEventCommits",
			   c => new
			   {
				   EventId = c.Guid( nullable: false ),
				   AggregateId = c.Guid( nullable: false ),
				   PublishedOn = c.DateTimeOffset( nullable: false ),
				   TransactionId = c.Guid( nullable: false ),
				   EventType = c.String( nullable: false ),
				   EventBlob = c.String( nullable: false ),
				   Version = c.Int( nullable: false ),
			   } )
			   .PrimaryKey( t => t.EventId );

			CreateTable(
				"dbo.People",
				c => new
				{
					Id = c.Guid( nullable: false ),
					Version = c.Int( nullable: false ),
					RowVersion = c.Binary( nullable: false, timestamp: true ),
					Name = c.String( nullable: false ),
				} )
				.PrimaryKey( t => t.Id );
		}

		public override void Down()
		{
			DropTable( "dbo.People" );
		}
	}
}
