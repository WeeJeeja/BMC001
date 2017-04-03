namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBooleansToBookingTableToTrackUserConfirmation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "GroupBooking", c => c.Boolean(nullable: false));
            AddColumn("dbo.Bookings", "AcceptedByUser", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "AcceptedByUser");
            DropColumn("dbo.Bookings", "GroupBooking");
        }
    }
}
