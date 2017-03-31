namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedCapacityFromBooking : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Bookings", "Capacity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Bookings", "Capacity", c => c.Int(nullable: false));
        }
    }
}
