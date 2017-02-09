namespace DataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAccountTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Guid(nullable: false, identity: true),
                        EmployeeNumber = c.Int(nullable: false),
                        Password = c.Binary(),
                    })
                .PrimaryKey(t => t.AccountId);
            
            AddColumn("dbo.Users", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Email");
            DropTable("dbo.Accounts");
        }
    }
}
