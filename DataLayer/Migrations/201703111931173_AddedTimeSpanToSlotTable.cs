namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTimeSpanToSlotTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Slots", "TimeFormat", c => c.Time(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slots", "TimeFormat");
        }
    }
}
