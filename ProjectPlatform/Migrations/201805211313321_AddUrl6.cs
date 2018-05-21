namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProjectStudentModels", "StudentName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProjectStudentModels", "StudentName");
        }
    }
}
