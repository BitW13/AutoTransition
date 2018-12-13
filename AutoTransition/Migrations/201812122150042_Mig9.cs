namespace AutoTransition.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "AddressInCity", c => c.String());
            DropColumn("dbo.Addresses", "Street");
            DropColumn("dbo.Addresses", "House");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Addresses", "House", c => c.String());
            AddColumn("dbo.Addresses", "Street", c => c.String());
            DropColumn("dbo.Addresses", "AddressInCity");
        }
    }
}
