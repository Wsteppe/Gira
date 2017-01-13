namespace Gira.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IssueHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IssueHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IssueId = c.Int(),
                        Status = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Issues", t => t.IssueId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.IssueId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IssueHistories", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IssueHistories", "IssueId", "dbo.Issues");
            DropIndex("dbo.IssueHistories", new[] { "UserId" });
            DropIndex("dbo.IssueHistories", new[] { "IssueId" });
            DropTable("dbo.IssueHistories");
        }
    }
}
