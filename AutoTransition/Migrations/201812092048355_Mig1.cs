namespace AutoTransition.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Mig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Country = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        House = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AutoRoutes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        StartAddressId = c.Guid(nullable: false),
                        EndAddressId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CargoDimensions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Length = c.Double(nullable: false),
                        Width = c.Double(nullable: false),
                        Hight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        AutoRouteId = c.Guid(nullable: false),
                        LoadDate = c.DateTime(nullable: false),
                        UnloadDate = c.DateTime(nullable: false),
                        TransportationTypeId = c.Guid(nullable: false),
                        CargoDimensionsId = c.Guid(nullable: false),
                        Weight = c.Double(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransportationTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TransportationType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        LastName = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        Password = c.String(),
                        UserClaimsId = c.Guid(nullable: false),
                        UserRole = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.UserClaims");
            DropTable("dbo.TransportationTypes");
            DropTable("dbo.Orders");
            DropTable("dbo.CargoDimensions");
            DropTable("dbo.AutoRoutes");
            DropTable("dbo.Addresses");
        }
    }
}
