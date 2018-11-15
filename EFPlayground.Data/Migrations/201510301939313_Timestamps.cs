namespace EFPlayground.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Timestamps : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Companies", "DateModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Customers", "DateModified", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "DateModified");
            DropColumn("dbo.Customers", "DateCreated");
            DropColumn("dbo.Companies", "DateModified");
            DropColumn("dbo.Companies", "DateCreated");
        }
    }
}
