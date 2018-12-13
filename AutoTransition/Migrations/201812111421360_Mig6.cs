namespace AutoTransition.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Records", "Email", c => c.String());
            DropColumn("dbo.Records", "NickName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Records", "NickName", c => c.String());
            DropColumn("dbo.Records", "Email");
        }
    }
}
