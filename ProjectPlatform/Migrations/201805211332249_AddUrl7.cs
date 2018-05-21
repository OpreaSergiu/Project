namespace ProjectPlatform.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUrl7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProjectStudentModels", "Grade", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProjectStudentModels", "Grade", c => c.String());
        }
    }
}
