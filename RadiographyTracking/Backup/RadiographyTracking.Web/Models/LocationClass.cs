using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace RadiographyTracking.Web.Models
{
    public class LocationClass
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String Location { get; set; }

        [NotMapped]
        public Guid FixedPatternPerformanceRowID { get; set; }

        [Include]
        [Association("SegmentClass", "ID", "LocationID")]
        public IEnumerable<SegmentClass> Segments { get; set; }
    }
}