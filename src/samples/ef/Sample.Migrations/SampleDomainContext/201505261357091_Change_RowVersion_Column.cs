namespace Sample.Migrations.SampleDomainContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_RowVersion_Column : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.People", "RowVersion");
            DropColumn("dbo.Companies", "RowVersion");
        }
        
        public override void Down()
        {
			AddColumn( "dbo.Companies", "RowVersion", c => c.Binary( nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion" ) );
            AddColumn("dbo.People", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
    }
}
