namespace RadiographyTracking.Web.Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Add_Viewing : DbMigration
    {
        public override void Up()
        {
            AddColumn("RGReports", "Viewing", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("RGReports", "Viewing");
        }
    }
}
