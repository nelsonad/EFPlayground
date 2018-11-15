namespace EFPlayground.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Collections : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateModified = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupCustomers",
                c => new
                    {
                        Group_Id = c.Guid(nullable: false),
                        Customer_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Group_Id, t.Customer_Id })
                .ForeignKey("dbo.Groups", t => t.Group_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Group_Id)
                .Index(t => t.Customer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GroupCustomers", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.GroupCustomers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.GroupCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.GroupCustomers", new[] { "Group_Id" });
            DropTable("dbo.GroupCustomers");
            DropTable("dbo.Groups");
        }
    }
}
