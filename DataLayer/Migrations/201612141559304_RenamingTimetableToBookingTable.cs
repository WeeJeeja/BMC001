namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenamingTimetableToBookingTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Timetables", "Resource_ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Timetables", "Slot_SlotId", "dbo.Slots");
            DropForeignKey("dbo.Timetables", "User_UserId", "dbo.Users");
            DropIndex("dbo.Timetables", new[] { "Resource_ResourceId" });
            DropIndex("dbo.Timetables", new[] { "Slot_SlotId" });
            DropIndex("dbo.Timetables", new[] { "User_UserId" });
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingId = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Capacity = c.Int(nullable: false),
                        Resource_ResourceId = c.Guid(),
                        Slot_SlotId = c.Guid(),
                        User_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.BookingId)
                .ForeignKey("dbo.Resources", t => t.Resource_ResourceId)
                .ForeignKey("dbo.Slots", t => t.Slot_SlotId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.Resource_ResourceId)
                .Index(t => t.Slot_SlotId)
                .Index(t => t.User_UserId);
            
            DropTable("dbo.Timetables");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Timetables",
                c => new
                    {
                        TimetableId = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Capacity = c.Int(nullable: false),
                        Resource_ResourceId = c.Guid(),
                        Slot_SlotId = c.Guid(),
                        User_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.TimetableId);
            
            DropForeignKey("dbo.Bookings", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Bookings", "Slot_SlotId", "dbo.Slots");
            DropForeignKey("dbo.Bookings", "Resource_ResourceId", "dbo.Resources");
            DropIndex("dbo.Bookings", new[] { "User_UserId" });
            DropIndex("dbo.Bookings", new[] { "Slot_SlotId" });
            DropIndex("dbo.Bookings", new[] { "Resource_ResourceId" });
            DropTable("dbo.Bookings");
            CreateIndex("dbo.Timetables", "User_UserId");
            CreateIndex("dbo.Timetables", "Slot_SlotId");
            CreateIndex("dbo.Timetables", "Resource_ResourceId");
            AddForeignKey("dbo.Timetables", "User_UserId", "dbo.Users", "UserId");
            AddForeignKey("dbo.Timetables", "Slot_SlotId", "dbo.Slots", "SlotId");
            AddForeignKey("dbo.Timetables", "Resource_ResourceId", "dbo.Resources", "ResourceId");
        }
    }
}
