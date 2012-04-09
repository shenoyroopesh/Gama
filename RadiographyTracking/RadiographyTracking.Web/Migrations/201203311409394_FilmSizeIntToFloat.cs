namespace RadiographyTracking.Web.Models.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class FilmSizeIntToFloat : DbMigration
    {
        public override void Up()
        {
            ChangeColumn("FilmSizes", "Width", c => c.Single(nullable: false));
            ChangeColumn("FilmSizes", "Length", c => c.Single(nullable: false));
            ChangeColumn("FilmSizes", "Area", c => c.Single(nullable: false));
            ChangeColumn("FilmTransactions", "Area", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            ChangeColumn("FilmTransactions", "Area", c => c.Int(nullable: false));
            ChangeColumn("FilmSizes", "Area", c => c.Int(nullable: false));
            ChangeColumn("FilmSizes", "Length", c => c.Int(nullable: false));
            ChangeColumn("FilmSizes", "Width", c => c.Int(nullable: false));
        }
    }
}
