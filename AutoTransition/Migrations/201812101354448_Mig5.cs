namespace AutoTransition.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        RecordDate = c.DateTime(nullable: false),
                        NickName = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Records");
        }
    }
}
