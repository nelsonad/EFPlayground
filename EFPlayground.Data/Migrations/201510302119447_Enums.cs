namespace EFPlayground.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Enums : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Source", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "Source");
        }
    }
}
