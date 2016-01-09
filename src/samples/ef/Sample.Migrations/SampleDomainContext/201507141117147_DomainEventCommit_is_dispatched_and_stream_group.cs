namespace Sample.Migrations.SampleDomainContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class DomainEventCommit_is_dispatched_and_stream_group : DbMigration
    {
        public override void Up()
        {
            AddColumn( "dbo.DomainEventCommits", "StreamGroup", c => c.String( nullable: false, defaultValue: "" ) );
            AddColumn( "dbo.DomainEventCommits", "IsDispatched", c => c.Boolean( nullable: false, defaultValue: false ) );
        }

        public override void Down()
        {
            DropColumn( "dbo.DomainEventCommits", "IsDispatched" );
            DropColumn( "dbo.DomainEventCommits", "StreamGroup" );
        }
    }
}
