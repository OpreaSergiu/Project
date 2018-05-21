namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInfoModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Role = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccountInfoModels");
        }
    }
}
