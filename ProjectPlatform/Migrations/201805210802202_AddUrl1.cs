namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AccountInfoModels", "CNP", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AccountInfoModels", "CNP");
        }
    }
}
