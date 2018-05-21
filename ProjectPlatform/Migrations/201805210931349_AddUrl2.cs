namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TeacherModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Class = c.String(),
                        Subject = c.String(),
                        SubjectId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TeacherModels");
        }
    }
}
