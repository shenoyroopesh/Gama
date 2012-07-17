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

        public int CoverageID { get; set; }

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
        public string ReportTypeNo { get; set; }

        [NotMapped]
        public String TotalArea
        {
            get
            {
                if (this.FinalRTReportRows == null)
                    return "0";

                return this.FinalRTReportRows
                    .Where(p=> p.RemarkText != "RETAKE")
                    .Sum(p => p.FilmSize.Area * p.FilmCount).ToString();
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
                    return di.GetFiles()
                        .Where(p => !(p.Name.ToLower().Contains("address") || p.Name.ToLower().Contains("change")))
                        .Select(p => p.Name).ToList();
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
        public Dictionary<String, float> EnergyAreas
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText != "RETAKE" //Roopesh: 30-Jun-2012
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmSize == null ? 0 : p.FilmSize.Area * p.FilmCount)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        public byte[] GetCompanyLogo()
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

        public byte[] GetCustomerLogo()
        {
            using (var ctx = new RadiographyContext())
            {
                var customer = ctx.FixedPatterns.Where(p => p.ID == this.FixedPatternID)
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