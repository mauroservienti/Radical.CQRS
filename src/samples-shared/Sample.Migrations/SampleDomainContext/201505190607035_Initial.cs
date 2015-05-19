namespace Sample.Migrations.SampleDomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DomainEventCommits",
                c => new
                    {
                        EventId = c.Guid(nullable: false),
                        AggregateId = c.Guid(nullable: false),
                        PublishedOn = c.DateTimeOffset(nullable: false, precision: 7),
                        TransactionId = c.Guid(nullable: false),
                        EventType = c.String(),
                        EventBlob = c.String(),
                        Version = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.PersonAddresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PersonId = c.Guid(nullable: false),
                        Street = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Version = c.Int(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonAddresses", "PersonId", "dbo.People");
            DropIndex("dbo.PersonAddresses", new[] { "PersonId" });
            DropTable("dbo.People");
            DropTable("dbo.PersonAddresses");
            DropTable("dbo.DomainEventCommits");
        }
    }
}
