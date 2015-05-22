namespace Sample.Migrations.SampleDomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_BornInfo_on_Person : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "Info_Where", c => c.String());
            AddColumn("dbo.People", "Info_When", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "Info_When");
            DropColumn("dbo.People", "Info_Where");
        }
    }
}
