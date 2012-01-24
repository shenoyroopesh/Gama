using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class RTStatusReportRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String FPNo { get; set; }
        [NotMapped]
        public String RTNo { get; set; }
        [NotMapped]
        public String Repairs { get; set; }
        [NotMapped]
        public String Retakes { get; set; }
        [NotMapped]
        public String Reshoots { get; set; }

        [NotMapped]
        public string Date { get; set; }
    }
}