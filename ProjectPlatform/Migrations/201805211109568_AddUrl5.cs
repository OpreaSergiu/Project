namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectStudentModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectName = c.String(),
                        Subject = c.String(),
                        Grade = c.String(),
                        StudentEmail = c.String(),
                        FolderPath = c.String(),
                        Class = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProjectStudentModels");
        }
    }
}
