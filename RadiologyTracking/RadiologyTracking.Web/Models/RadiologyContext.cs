using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RadiologyTracking.Web.Models
{
    public class RadiologyContext : DbContext
    {
        public RadiologyContext()
            : base("name=ApplicationServices")
        {
            if (HttpContext.Current == null)
            {
                Database.SetInitializer<RadiologyContext>(null);
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
        public DbSet<Observation> Observations { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<RGReport> RGReports { get; set; }
        public DbSet<RGReportSource> RGReportSources { get; set; }
        public DbSet<RGReportRow> RGReportRows { get; set; }
        public DbSet<RGReportRowType> RGReportRowTypes { get; set; }
        public DbSet<RGStatus> RGStatuses { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<Welder> Welders { get; set; }
        public DbSet<ThicknessRangeForEnergy> ThicknessRangesForEnergy { get; set; }
    }
}