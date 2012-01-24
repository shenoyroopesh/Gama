using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;
using RadiologyTracking.Web.Utility;

namespace RadiologyTracking.Web.Models
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
    }
}