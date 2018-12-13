namespace AutoTransition.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig8 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TransportationTypes", "TransportationType", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TransportationTypes", "TransportationType", c => c.String());
        }
    }
}
