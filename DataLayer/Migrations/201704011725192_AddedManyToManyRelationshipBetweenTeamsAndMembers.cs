namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedManyToManyRelationshipBetweenTeamsAndMembers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.Users", new[] { "Team_TeamId" });
            CreateTable(
                "dbo.TeamUsers",
                c => new
                    {
                        Team_TeamId = c.Guid(nullable: false),
                        User_UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Team_TeamId, t.User_UserId })
                .ForeignKey("dbo.Teams", t => t.Team_TeamId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId, cascadeDelete: true)
                .Index(t => t.Team_TeamId)
                .Index(t => t.User_UserId);
            
            DropColumn("dbo.Users", "Team_TeamId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Team_TeamId", c => c.Guid());
            DropForeignKey("dbo.TeamUsers", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.TeamUsers", "Team_TeamId", "dbo.Teams");
            DropIndex("dbo.TeamUsers", new[] { "User_UserId" });
            DropIndex("dbo.TeamUsers", new[] { "Team_TeamId" });
            DropTable("dbo.TeamUsers");
            CreateIndex("dbo.Users", "Team_TeamId");
            AddForeignKey("dbo.Users", "Team_TeamId", "dbo.Teams", "TeamId");
        }
    }
}
