using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class SegmentClass
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String Segment { get; set; }

        [NotMapped]
        public String Observations { get; set; }

        [NotMapped]
        public string RemarkText { get; set; }

        [NotMapped]
        public Guid LocationID { get; set; }
    }
}