namespace EFPlayground.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER DATABASE CURRENT SET ALLOW_SNAPSHOT_ISOLATION ON", true);

            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        CompanyId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Customers", new[] { "CompanyId" });
            DropTable("dbo.Customers");
            DropTable("dbo.Companies");
        }
    }
}
