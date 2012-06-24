using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;
using RadiographyTracking.Web.Utility;
using System.Drawing;
using System.IO;
using System.Data.Entity;

namespace RadiographyTracking.Web.Models
{
    /// <summary>
    /// This class represents a single Radiography report entry, which forms the basis of all the work done 
    /// by the users of this software
    /// </summary>
    public class RGReport
    {
        /// <summary>
        /// Default constructor doesn't do much, here only for RIA services to work fine
        /// </summary>
        public RGReport()
        {
        }

        /// <summary>
        /// This constructor creates an initial RGReport based on an existing fpTemplate. It does not check whether
        /// an existing RG Report exists, so make sure of that before calling this method
        /// </summary>
        /// <param name="fpTemplate"></param>
        /// <param name="ctx">Database Context with reference which to create the object</param>
        public RGReport(FixedPatternTemplate fpTemplate, string RTNo, string ReportNo, RadiographyContext ctx)
        {
            //shallow copy properties
            fpTemplate.CopyTo(this, "ID");
            this.DateOfTest = this.ReportDate = DateTime.Now;
            this.Shift = Shift.getShift("DAY", ctx); //defaulting so it can be saved
            this.Status = RGStatus.getStatus("PENDING", ctx);
            this.RTNo = RTNo;
            this.ReportNo = ReportNo;

            RGReportRowType freshRowType = RGReportRowType.getRowType("FRESH", ctx);
            if (fpTemplate.FPTemplateRows == null) return;

            this.RGReportRows = new List<RGReportRow>();

            //some default values as suggested by Shankaran (10-Apr-2012)
            this.Film = "AGFA D7";
            this.LeadScreen = "0.1mm";

            //since this is the first report for this FP and RT No
            this.First = true;
            this.RowsDeleted = false;

            foreach (var row in fpTemplate.FPTemplateRows.OrderBy(p => p.SlNo))
            {
                var rgReportRow = new RGReportRow
                                            {
                                                RowType = freshRowType,
                                                Energy = Energy.getEnergyForThickness(row.Thickness, ctx),
                                                Observations = " ", //for grid to work fine
                                                FilmCount = 1 // default for the new film count
                                            };
                row.CopyTo(rgReportRow, "ID,FilmSizeString");

                //for future reports, so that ordering can be done on this basis
                rgReportRow.FPSLNo = row.SlNo;

                this.RGReportRows.Add(rgReportRow);
            }
        }

        /// <summary>
        /// This constructor creates an follow up RG report based on an existing RG Report for the same RT No. 
        /// </summary>
        /// <param name="reportNo"> </param>
        /// <param name="ctx">Database Context with reference which to create the object</param>
        /// <param name="parentRGReports"> </param>
        public RGReport(List<RGReport> parentRGReports, String reportNo, RadiographyContext ctx)
        {
            if (reportNo == null) throw new ArgumentNullException("reportNo");
            var latestParent = parentRGReports.OrderByDescending(p => p.ReportDate).First();

            //all rows with some remark
            var rows = from r in parentRGReports.SelectMany(p => p.RGReportRows)
                       where r.Remark != null
                       select r;

            //latest row for each location and segment combination
            var latestRows = rows.Where(p => rows != null && !rows.Any(r => r.Location == p.Location &&
                                                                            r.Segment == p.Segment &&
                                                                            r.RGReport.ReportDate > p.RGReport.ReportDate));

            //all those that are not yet acceptable
            var neededRows = latestRows
                             .Where(p => p.Remark.Value != "ACCEPTABLE")
                             .OrderBy(p => p.FPSLNo);

            latestParent.CopyTo(this, "ID,ReportDate,RGReportRows");
            this.ReportDate = DateTime.Now;
            this.ReportNo = reportNo;
            this.RGReportRows = new List<RGReportRow>();
            this.RowsDeleted = false;

            //since this is at least the second report
            this.First = false;

            //only those rows to be copied from entire history which do not have acceptable against that particular location and segment
            var slNo = 1;

            foreach (var row in neededRows)
            {
                if (row.Remark.Value == "ACCEPTABLE") continue;

                //row type for this row depends on the corresponding parent rows remarks
                var reportRow = new RGReportRow()
                {
                    RowType = RGReportRowType.getRowType(row.Remark.Value, ctx),
                    Observations = " "
                };
                row.CopyTo(reportRow,
                    "ID,RGReport,Observations,Remark,RemarkText,ObservationsText," +
                    "Technician,TechnicianText,Welder,WelderText,RowType,ReportNo");
                reportRow.SlNo = slNo++;
                this.RGReportRows.Add(reportRow);
            }
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int FixedPatternID { get; set; }

        //This is used for determining whether this is the first report or one of the later ones. This is important because deleting a row in the first
        //report is handled differently compared to deleting a row in the later reports
        public bool First { get; set; }

        //has a flag whether rows in this particular report have been deleted. Just to ensure that if this is not the first report and this has rows deleted
        //then this can never be the final casting
        public bool RowsDeleted { get; set; }
        public FixedPattern FixedPattern { get; set; }
        public int CoverageID { get; set; }
        public Coverage Coverage { get; set; }
        public String LeadScreen { get; set; }
        public String SourceSize { get; set; }
        public String RTNo { get; set; }
        public String ReportNo { get; set; }
        public String HeatNo { get; set; }
        public String ProcedureRef { get; set; }
        public String Specifications { get; set; }
        public DateTime ReportDate { get; set; }
        public String Film { get; set; }
        public DateTime DateOfTest { get; set; }
        public int? ShiftID { get; set; }
        public Shift Shift { get; set; }
        public String EvaluationAsPer { get; set; }
        public String AcceptanceAsPer { get; set; }
        public String DrawingNo { get; set; }
        public int StatusID { get; set; }
        public RGStatus Status { get; set; }

        [Include]
        public ICollection<RGReportRow> RGReportRows { get; set; }
        public String Result { get; set; }

        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, float> EnergyAreas
        {
            get
            {
                if (RGReportRows == null)
                    return null;

                var summary = from r in RGReportRows
                              group r by r.EnergyText into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Select(p => p.FilmSize == null ? 0 : p.FilmSize.Area).Sum()
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public float TotalArea
        {
            get
            {
                return this.RGReportRows == null ? 0 : this.RGReportRows.Select(p => p.FilmSize == null ? 0 : p.FilmSize.Area * p.FilmCount).Sum();
            }
        }

        [NotMapped]
        public bool CanDelete
        {
            get
            {
                using (var ctx = new RadiographyContext())
                {
                    //check if there is an report for this RTNo newer that this report, if so can't delete
                    var newerReportCount = ctx.RGReports.Count(p => p.RTNo == this.RTNo &&
                                                                    p.ReportNo != this.ReportNo &&
                                                                    p.ReportDate > this.ReportDate);

                    return newerReportCount == 0;
                }
            }
        }


        public byte[] getCompanyLogo()
        {
            using (var ctx = new RadiographyContext())
            {
                var company = ctx.Companies.Include(p => p.Logo).First();
                if (company.Logo != null)
                {
                    return company.Logo.FileData;
                }
            }
            return null;
        }
    }
}