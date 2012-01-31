using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace RadiographyTracking.Web.Models
{
    public class FixedPatternPerformanceRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String RTNo { get; set; }

        [NotMapped]
        public DateTime Date { get; set; }

        [Include]
        [Association("LocationClass", "ID", "FixedPatternPerformanceRowID")]
        public IEnumerable<LocationClass> Locations { get; set; }
    }
}