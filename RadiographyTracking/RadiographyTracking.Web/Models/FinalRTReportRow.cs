using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FinalRTReportRow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [NotMapped]
        public int SlNo { get; set; }
        [NotMapped]
        public String Location { get; set; }
        [NotMapped]
        public String Segment { get; set; }
        [NotMapped]
        public int Thickness { get; set; }

        public int EnergyID { get; set; }
        public Energy Energy { get; set; }
        [NotMapped]
        public int SFD { get; set; }
        [NotMapped]
        public String Designation { get; set; }
        [NotMapped]
        public String Sensitivity { get; set; }
        [NotMapped]
        public String Density { get; set; }
        
        public int FilmSizeID { get; set; }
        public FilmSize FilmSize { get; set; }
        
        [NotMapped]
        public String Observations { get; set; }
        
        [NotMapped]
        public int? RemarkID { get; set; }
        [NotMapped]
        public Remark Remark { get; set; }
        [NotMapped]
        public int? TechnicianID { get; set; }
        [NotMapped]
        public Technician Technician { get; set; }
        [NotMapped]
        public int? WelderID { get; set; }
        [NotMapped]
        public Welder Welder { get; set; }
        [NotMapped]
        public int FinalRTReportID { get; set; }

        /// <summary>
        /// This will determine the cause for this row in the first place - necessary to track this separately from remark
        /// 
        /// For eg, in a first report, the cause for the row will be FRESH but the remark could be REPAIR. In this case, the next report will have a corresponding row 
        /// whose cause is REPAIR which can again have different REMARK such as REPAIR or RESHOOT or ACCEPTABLE
        /// </summary>
        [NotMapped]
        public RGReportRowType RowType { get; set; }
        [NotMapped]
        public int RowTypeID { get; set; }

        [NotMapped]
        public String FilmSizeString
        {
            get;
            set;
        }

        /// <summary>
        /// Only for setting or seeing Remark by using a string value
        /// </summary>
        [NotMapped]
        public string RemarkText
        {
            get;
            set;
        }

        /// <summary>
        /// Only for setting or seeing Welder by using a string value
        /// </summary>
        [NotMapped]
        public string WelderText
        {
            get;
            set;
        }

        /// <summary>
        /// Only for setting or seeing Technician by using a string value
        /// </summary>
        [NotMapped]
        public string TechnicianText
        {
            get;
            set;
        }
    }
}