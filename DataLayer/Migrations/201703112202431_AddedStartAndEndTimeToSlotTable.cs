namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStartAndEndTimeToSlotTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slots", "StartTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Slots", "EndTime", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Slots", "TimeFormat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Slots", "TimeFormat", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Slots", "EndTime");
            DropColumn("dbo.Slots", "StartTime");
        }
    }
}
