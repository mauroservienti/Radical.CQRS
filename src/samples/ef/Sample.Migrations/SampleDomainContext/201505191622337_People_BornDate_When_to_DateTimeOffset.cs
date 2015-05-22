namespace Sample.Migrations.SampleDomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class People_BornDate_When_to_DateTimeOffset : DbMigration
    {
        public override void Up()
        {
			Sql( "ALTER TABLE [dbo].[People] DROP CONSTRAINT [DF__People__Info_Whe__173876EA]" );
            AlterColumn("dbo.People", "Info_When", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.People", "Info_When", c => c.DateTime(nullable: false));
			Sql( "ALTER TABLE [dbo].[People] ADD  DEFAULT ('1900-01-01T00:00:00.000') FOR [Info_When]" );
        }
    }
}
