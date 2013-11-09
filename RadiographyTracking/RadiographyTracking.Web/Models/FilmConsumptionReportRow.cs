using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FilmConsumptionReportRow
    {
        [Key]
        public Guid ID { get; set; }
        [NotMapped]
        public String ReportNo { get; set; }
        [NotMapped]
        public String Date { get; set; }
        [NotMapped]
        public String FPNo { get; set; }
        [NotMapped]
        public String ReportTypeAndNo { get; set; }
        [NotMapped]
        public String RTNo { get; set; }
        [NotMapped]
        public String Energy { get; set; }
        [NotMapped]
        public String RowType { get; set; }
        [NotMapped]
        public float Area { get; set; }
    }
}