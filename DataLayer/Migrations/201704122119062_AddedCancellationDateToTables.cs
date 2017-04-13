namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCancellationDateToTables : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CancellationDate", c => c.DateTime());
            AddColumn("dbo.Resources", "CancellationDate", c => c.DateTime());
            AddColumn("dbo.Slots", "CancellationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Slots", "CancellationDate");
            DropColumn("dbo.Resources", "CancellationDate");
            DropColumn("dbo.Users", "CancellationDate");
        }
    }
}
