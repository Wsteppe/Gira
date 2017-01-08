namespace Gira.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRequirements : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ResponsibleUserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ResponsibleUserId" });
            CreateTable(
                "dbo.Issues",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Description = c.String(),
                        PriorityCode = c.Int(),
                        IssueStatusCode = c.Int(nullable: false),
                        Occurrence = c.DateTime(),
                        ResponsibleUserId = c.String(maxLength: 128),
                        CreatorId = c.String(maxLength: 128),
                        ManagerId = c.String(maxLength: 128),
                        Registered = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.ManagerId)
                .ForeignKey("dbo.AspNetUsers", t => t.ResponsibleUserId)
                .Index(t => t.ResponsibleUserId)
                .Index(t => t.CreatorId)
                .Index(t => t.ManagerId);
            
            DropColumn("dbo.AspNetUsers", "ResponsibleUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ResponsibleUserId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Issues", "ResponsibleUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "ManagerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Issues", "CreatorId", "dbo.AspNetUsers");
            DropIndex("dbo.Issues", new[] { "ManagerId" });
            DropIndex("dbo.Issues", new[] { "CreatorId" });
            DropIndex("dbo.Issues", new[] { "ResponsibleUserId" });
            DropTable("dbo.Issues");
            CreateIndex("dbo.AspNetUsers", "ResponsibleUserId");
            AddForeignKey("dbo.AspNetUsers", "ResponsibleUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
