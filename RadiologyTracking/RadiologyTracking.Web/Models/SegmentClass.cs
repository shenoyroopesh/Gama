using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
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
        public Guid LocationID { get; set; }
    }
}