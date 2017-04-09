namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBooleanFlagToBookingTableForGroupBookings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "GroupBooking", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "GroupBooking");
        }
    }
}
