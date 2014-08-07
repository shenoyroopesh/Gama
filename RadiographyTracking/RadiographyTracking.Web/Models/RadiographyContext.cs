using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RadiographyTracking.Web.Models
{
    public class RadiographyContext : DbContext
    {
        public RadiographyContext()
            : base("name=ApplicationServices")
        {
            if (HttpContext.Current == null)
            {
                Database.SetInitializer<RadiographyContext>(null);
            }
        }

        public DbSet<Change> Changes { get; set; }
        public DbSet<Coverage> Coverages { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Energy> Energies { get; set; }
        public DbSet<FilmSize> FilmSizes { get; set; }
        public DbSet<FilmTransaction> FilmTransactions { get; set; }
        public DbSet<FixedPattern> FixedPatterns { get; set; }
        public DbSet<FixedPatternTemplate> FixedPatternTemplates { get; set; }
        public DbSet<Foundry> Foundries { get; set; }
        public DbSet<FPTemplateRow> FPTemplateRows { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<RGReport> RGReports { get; set; }
        public DbSet<RGReportRow> RGReportRows { get; set; }
        public DbSet<RGReportRowType> RGReportRowTypes { get; set; }
        public DbSet<RGStatus> RGStatuses { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Welder> Welders { get; set; }
        public DbSet<ThicknessRangeForEnergy> ThicknessRangesForEnergy { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<RetakeReason> RetakeReasons { get; set; }

        //following are added only for reports, though they will create a few empty tables in the db, there
        //is no way around it for now. This is the path of least resistance

        public DbSet<FilmStockReportRow> FilmStockReportRows { get; set; }
        public DbSet<LocationClass> Locations { get; set; }
        public DbSet<SegmentClass> Segments { get; set; }
        public DbSet<FixedPatternPerformanceRow> FixedPatternPerformanceRows { get; set; }
        public DbSet<ShiftWisePerformanceRow> ShiftWisePerformanceRows { get; set; }
        public DbSet<FilmAreaRow> FilmAreaRows { get; set; }
        public DbSet<FilmConsumptionReportRow> FilmConsumptionReportRows { get; set; }
        public DbSet<RTStatusReportRow> RTStatusReportRows { get; set; }
        public DbSet<FinalRTReport> FinalRTReports { get; set; }
        public DbSet<FinalRTReportRow> FinalRTReportRows { get; set; }
        public DbSet<RetakeReasonReportRow> RetakeReasonReportRows { get; set; }

        public DbSet<Period> Periods { get; set; }
    }
}