namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAnUnconfirmedBookingsTableForStaging : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnconfirmedBookings",
                c => new
                    {
                        UnconfirmedBookingId = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        BookedBy_UserId = c.Guid(),
                        Resource_ResourceId = c.Guid(),
                        Slot_SlotId = c.Guid(),
                        User_UserId = c.Guid(),
                    })
                .PrimaryKey(t => t.UnconfirmedBookingId)
                .ForeignKey("dbo.Users", t => t.BookedBy_UserId)
                .ForeignKey("dbo.Resources", t => t.Resource_ResourceId)
                .ForeignKey("dbo.Slots", t => t.Slot_SlotId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.BookedBy_UserId)
                .Index(t => t.Resource_ResourceId)
                .Index(t => t.Slot_SlotId)
                .Index(t => t.User_UserId);
            
            AddColumn("dbo.Bookings", "BookedBy_UserId", c => c.Guid());
            CreateIndex("dbo.Bookings", "BookedBy_UserId");
            AddForeignKey("dbo.Bookings", "BookedBy_UserId", "dbo.Users", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnconfirmedBookings", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.UnconfirmedBookings", "Slot_SlotId", "dbo.Slots");
            DropForeignKey("dbo.UnconfirmedBookings", "Resource_ResourceId", "dbo.Resources");
            DropForeignKey("dbo.UnconfirmedBookings", "BookedBy_UserId", "dbo.Users");
            DropForeignKey("dbo.Bookings", "BookedBy_UserId", "dbo.Users");
            DropIndex("dbo.UnconfirmedBookings", new[] { "User_UserId" });
            DropIndex("dbo.UnconfirmedBookings", new[] { "Slot_SlotId" });
            DropIndex("dbo.UnconfirmedBookings", new[] { "Resource_ResourceId" });
            DropIndex("dbo.UnconfirmedBookings", new[] { "BookedBy_UserId" });
            DropIndex("dbo.Bookings", new[] { "BookedBy_UserId" });
            DropColumn("dbo.Bookings", "BookedBy_UserId");
            DropTable("dbo.UnconfirmedBookings");
        }
    }
}
