namespace AutoTransition.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AutoRoutes", "Distance", c => c.Double(nullable: false));
            DropColumn("dbo.Addresses", "Country");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "Country", c => c.String());
            DropColumn("dbo.AutoRoutes", "Distance");
        }
    }
}
