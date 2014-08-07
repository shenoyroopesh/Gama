using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class RetakeReasonReportRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String FPNo { get; set; }

        [NotMapped]
        public string Coverage { get; set; }

        [NotMapped]
        public String RTNo { get; set; }

        [NotMapped]
        public String Location { get; set; }

        [NotMapped]
        public String Segment { get; set; }

        [NotMapped]
        public String RetakeReason { get; set; }
    }
}