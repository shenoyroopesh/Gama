using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;
using RadiographyTracking.Web.Utility;
using System.Data.Entity;
using System.IO;

namespace RadiographyTracking.Web.Models
{
    /// <summary>
    /// This class represents a Latest Radiography Report entry, which forms the basis of all the work done 
    /// by the users of this software
    /// </summary>
    public class FinalRTReport
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int FixedPatternID { get; set; }

        public FixedPattern FixedPattern { get; set; }
        [NotMapped]
        public int CoverageID { get; set; }
        [NotMapped]
        public Coverage Coverage { get; set; }
        [NotMapped]
        public String LeadScreen { get; set; }
        [NotMapped]
        public String SourceSize { get; set; }
        [NotMapped]
        public String RTNo { get; set; }
        [NotMapped]
        public String ReportNo { get; set; }
        [NotMapped]
        public String HeatNo { get; set; }
        [NotMapped]
        public String ProcedureRef { get; set; }
        [NotMapped]
        public String Specifications { get; set; }
        [NotMapped]
        public String ReportDate { get; set; }
        [NotMapped]
        public String Film { get; set; }
        [NotMapped]
        public String DateOfTest { get; set; }

        public int ShiftID { get; set; }
        public Shift Shift { get; set; }

        [NotMapped]
        public String EvaluationAsPer { get; set; }
        [NotMapped]
        public String AcceptanceAsPer { get; set; }
        [NotMapped]
        public String DrawingNo { get; set; }

        public int StatusID { get; set; }
        public RGStatus Status { get; set; }

        [NotMapped]
        [Include]
        [Association("FinalRTReport", "ID", "FinalRTReportID")]
        public ICollection<FinalRTReportRow> FinalRTReportRows { get; set; }

        [NotMapped]
        public String Result { get; set; }

        [NotMapped]
        public String TotalArea
        {
            get
            {
                if (this.FinalRTReportRows == null)
                    return "0";

                return this.FinalRTReportRows.Select(p => p.FilmSize.Area).Sum().ToString();
            }
        }

        /// <summary>
        /// Will give the list of report templates
        /// </summary>
        [NotMapped]
        public List<String> ReportTemplatesList
        {
            get
            {
                string absolutepath = HttpContext.Current.Server.MapPath("~/ReportTemplates/");

                if (Directory.Exists(absolutepath))
                {
                    DirectoryInfo di = new DirectoryInfo(absolutepath);
                    return di.GetFiles().Select(p => p.Name).ToList();
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, int> EnergyAreas
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                Dictionary<String, int> rows = new Dictionary<string, int>();

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Select(p => p.FilmSize == null ? 0 : p.FilmSize.Area).Sum()
                              };

                foreach (var s in summary)
                {
                    rows.Add(s.Energy, s.Area);
                }
                return rows;
            }
        }

        public byte[] getCompanyLogo()
        {
            using (RadiographyContext ctx = new RadiographyContext())
            {
                Company company = ctx.Companies.Include(p => p.Logo).First();
                if (company.Logo != null)
                {
                    return company.Logo.FileData;
                }
            }
            return null;
        }

        public byte[] getCustomerLogo()
        {
            using (RadiographyContext ctx = new RadiographyContext())
            {
                Customer customer = ctx.FixedPatterns.Where(p => p.ID == this.FixedPatternID)
                                        .Include(p => p.Customer.Logo)
                                        .First()
                                        .Customer;
                if (customer.Logo != null)
                {
                    return customer.Logo.FileData;
                }
            }
            return null;
        }

    }
}