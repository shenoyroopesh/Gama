using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

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
        [NotMapped]
        public float AreaSingleFilm { get; set; }
        [NotMapped]
        public float AreaAdditionalFilm { get; set; }
        [NotMapped]
        public int ReshootNo { get; set; }
        [NotMapped]
        public float AreaInCo { get; set; }
        [NotMapped]
        public float AreaInIr { get; set; }
        [NotMapped]
        public String DateOfTest { get; set; }
    }
}