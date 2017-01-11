namespace Gira.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RevisedManager : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Issues", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.Issues", new[] { "ManagerId" });
            AddColumn("dbo.AspNetUsers", "ManagerId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ManagerId");
            AddForeignKey("dbo.AspNetUsers", "ManagerId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Issues", "ManagerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Issues", "ManagerId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.AspNetUsers", "ManagerId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ManagerId" });
            DropColumn("dbo.AspNetUsers", "ManagerId");
            CreateIndex("dbo.Issues", "ManagerId");
            AddForeignKey("dbo.Issues", "ManagerId", "dbo.AspNetUsers", "Id");
        }
    }
}
