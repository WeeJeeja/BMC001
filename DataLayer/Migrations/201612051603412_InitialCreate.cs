namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        Category = c.String(),
                        Capacity = c.Int(nullable: false),
                        Location = c.String(),
                    })
                .PrimaryKey(t => t.ResourceId);
            
            CreateTable(
                "dbo.Slots",
                c => new
                    {
                        SlotId = c.Guid(nullable: false, identity: true),
                        Time = c.String(),
                    })
                .PrimaryKey(t => t.SlotId);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamId = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Colour = c.String(),
                    })
                .PrimaryKey(t => t.TeamId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Guid(nullable: false, identity: true),
                        EmployeeNumber = c.Int(nullable: false),
                        Forename = c.String(),
                        Surname = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        JobTitle = c.String(),
                        IsLineManager = c.Boolean(nullable: false),
                        IsAdministrator = c.Boolean(nullable: false),
                        LineManager_UserId = c.Guid(),
                        Team_TeamId = c.Guid(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.LineManager_UserId)
                .ForeignKey("dbo.Teams", t => t.Team_TeamId)
                .Index(t => t.LineManager_UserId)
                .Index(t => t.Team_TeamId);
            
            CreateTable(
                "dbo.Timetables",
                c => new
                    {
                        TimetableId = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Resource_ResourceId = c.Guid(),
                        Slot_SlotId = c.Guid(),
                        User_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.TimetableId)
                .ForeignKey("dbo.Resources", t => t.Resource_ResourceId)
                .ForeignKey("dbo.Slots", t => t.Slot_SlotId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Resource_ResourceId)
                .Index(t => t.Slot_SlotId)
                .Index(t => t.User_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Timetables", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Timetables", "Slot_SlotId", "dbo.Slots");
            DropForeignKey("dbo.Timetables", "Resource_ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Users", "Team_TeamId", "dbo.Teams");
            DropForeignKey("dbo.Users", "LineManager_UserId", "dbo.Users");
            DropIndex("dbo.Timetables", new[] { "User_UserId" });
            DropIndex("dbo.Timetables", new[] { "Slot_SlotId" });
            DropIndex("dbo.Timetables", new[] { "Resource_ResourceId" });
            DropIndex("dbo.Users", new[] { "Team_TeamId" });
            DropIndex("dbo.Users", new[] { "LineManager_UserId" });
            DropTable("dbo.Timetables");
            DropTable("dbo.Users");
            DropTable("dbo.Teams");
            DropTable("dbo.Slots");
            DropTable("dbo.Resources");
        }
    }
}
