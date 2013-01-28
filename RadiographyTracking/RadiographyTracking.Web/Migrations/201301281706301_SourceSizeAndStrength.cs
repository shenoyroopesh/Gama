namespace RadiographyTracking.Web.Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class SourceSizeAndStrength : DbMigration
    {
        public override void Up()
        {
            AddColumn("RGReports", "Strength", c => c.String());
            RenameColumn("RGReports", "SourceSize", "Source");
            AddColumn("RGReports", "SourceSize", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("RGReports", "SourceSize");
            RenameColumn("RGReports", "Source", "SourceSize");
            DropColumn("RGReports", "Strength");
        }
    }
}
