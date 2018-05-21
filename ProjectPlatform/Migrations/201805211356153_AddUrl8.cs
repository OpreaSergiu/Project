namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl8 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        UserEmail = c.String(),
                        UserName = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CommentModels");
        }
    }
}
