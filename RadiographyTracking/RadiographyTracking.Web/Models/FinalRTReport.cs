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
        public String LeadScreenBack { get; set; }
        [NotMapped]
        public String Strength { get; set; }
        [NotMapped]
        public String Source { get; set; }
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

        [NotMapped]
        public String EndCustomerName { get; set; }

        public int ShiftID { get; set; }
        public Shift Shift { get; set; }

        private string evaluationAsPer;
        [NotMapped]
        public string EvaluationAsPer
        {
            get
            {

                if (FinalRTReportRows == null)
                    return string.Empty;
                else
                {
                    try
                    {
                        int range1 = 0, range2 = 0, range3 = 0;

                        foreach (var row in FinalRTReportRows)
                        {
                            var thicknessValue = Convert.ToInt32(row.ThicknessRange.Split('-').ToList().Select(k => int.Parse(k.Trim())).Average());
                            if (thicknessValue >= 0 && thicknessValue <= 50)
                                range1++;
                            if (thicknessValue >= 51 && thicknessValue <= 114)
                                range2++;
                            if (thicknessValue >= 115 && thicknessValue <= 305)
                                range3++;
                        }

                        var eval = "ASTM";
                        if (range1 > 0)
                            eval = eval + " E446";
                        if (range2 > 0)
                            eval = eval == "ASTM" ? eval + " E186" : eval + " / E186 ";
                        if (range3 > 0)
                            eval = eval == "ASTM" ? eval + " E280" : eval + " / E280 ";

                        return eval == "ASTM" ? string.Empty : eval;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            set { evaluationAsPer = value; }

        }
        [NotMapped]
        public String AcceptanceAsPer { get; set; }
        [NotMapped]
        public String DrawingNo { get; set; }

        public int StatusID { get; set; }
        public RGStatus Status { get; set; }

        [NotMapped]
        public String Viewing { get; set; }

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
                    .Where(p => p.RemarkText != "RETAKE")
                    .Sum(p => p.FilmArea).ToString();
            }
        }

        [NotMapped]
        public String TotalFilmCount
        {
            get
            {
                return FinalRTReportRows == null
                           ? "0"
                           : FinalRTReportRows
                                 .Sum(p => p.FilmCount)
                                 .ToString();
            }
        }

        [NotMapped]
        [Exclude]
        public string ExposedTotalArea
        {
            get
            {
                return FinalRTReportRows == null
                           ? "0"
                           : FinalRTReportRows
                                 .Sum(p => p.FilmArea)
                                 .ToString();
            }
        }

        [NotMapped]
        [Exclude]
        public string RetakeTotalArea
        {
            get
            {
                return FinalRTReportRows == null
                           ? "0"
                           : FinalRTReportRows
                                 .Where(p => p.RemarkText == "RETAKE")
                                 .Sum(p => p.FilmArea)
                                 .ToString();
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
                                  Area = g.Sum(p => p.FilmArea)
                              }; //TODO: note this exact logic is present in RGReport as well. Whenever making changes here make there too. 

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> ExposedEnergyAreas
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmArea)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> RetakeEnergyAreas
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText == "RETAKE"
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmArea)
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

        [NotMapped]
        public String TotalAreaInCms
        {
            get
            {
                if (this.FinalRTReportRows == null)
                    return "0";

                return this.FinalRTReportRows
                    .Where(p => p.RemarkText != "RETAKE")
                    .Sum(p => p.FilmAreaInCms).ToString();
            }
        }

        [NotMapped]
        [Exclude]
        public string ExposedTotalAreaInCms
        {
            get
            {
                return FinalRTReportRows == null
                           ? "0"
                           : FinalRTReportRows
                                 .Sum(p => p.FilmAreaInCms)
                                 .ToString();
            }
        }

        [NotMapped]
        [Exclude]
        public string RetakeTotalAreaInCms
        {
            get
            {
                return FinalRTReportRows == null
                           ? "0"
                           : FinalRTReportRows
                                 .Where(p => p.RemarkText == "RETAKE")
                                 .Sum(p => p.FilmAreaInCms)
                                 .ToString();
            }
        }

        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, float> EnergyAreasInCms
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
                                  Area = g.Sum(p => p.FilmAreaInCms)
                              }; //TODO: note this exact logic is present in RGReport as well. Whenever making changes here make there too. 

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> ExposedEnergyAreasInCms
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaInCms)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> RetakeEnergyAreasInCms
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText == "RETAKE"
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaInCms)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        ///////////////////////

        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, float> EnergyAreasForFirstFilm
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
                                  Area = g.Sum(p => p.FilmAreaFirstFilm)
                              }; //TODO: note this exact logic is present in RGReport as well. Whenever making changes here make there too. 

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, float> EnergyAreasForAdditionalFilm
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
                                  Area = g.Sum(p => p.FilmAreaAdditionalFilm)
                              }; //TODO: note this exact logic is present in RGReport as well. Whenever making changes here make there too. 

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, float> EnergyAreasInCmsForFirstFilm
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
                                  Area = g.Sum(p => p.FilmAreaInCmsFirstFilm)
                              }; //TODO: note this exact logic is present in RGReport as well. Whenever making changes here make there too. 

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        /// <summary>
        /// Calculated field that gets the energy area for this particular report
        /// </summary>
        [NotMapped]
        [Exclude]
        public Dictionary<String, float> EnergyAreasInCmsForAdditionalFilm
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
                                  Area = g.Sum(p => p.FilmAreaInCmsAdditionalFilm)
                              }; //TODO: note this exact logic is present in RGReport as well. Whenever making changes here make there too. 

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> ExposedEnergyAreasForFirstFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaFirstFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> ExposedEnergyAreasForAdditionalFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaAdditionalFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> ExposedEnergyAreasInCmsForFirstFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaInCmsFirstFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> ExposedEnergyAreasInCmsForAdditionalFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaInCmsAdditionalFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> RetakeEnergyAreasForFirstFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText == "RETAKE"
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaFirstFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> RetakeEnergyAreasForAdditionalFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText == "RETAKE"
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaAdditionalFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> RetakeEnergyAreasInCmsForFirstFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText == "RETAKE"
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaInCmsFirstFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }

        [NotMapped]
        [Exclude]
        public Dictionary<String, float> RetakeEnergyAreasInCmsForAdditionalFilm
        {
            get
            {
                if (FinalRTReportRows == null)
                    return null;

                var summary = from r in FinalRTReportRows
                              where r.RemarkText == "RETAKE"
                              group r by r.Energy.Name into g
                              select new
                              {
                                  Energy = g.Key,
                                  Area = g.Sum(p => p.FilmAreaInCmsAdditionalFilm)
                              };

                return summary.ToDictionary(s => s.Energy, s => s.Area);
            }
        }
    }
}