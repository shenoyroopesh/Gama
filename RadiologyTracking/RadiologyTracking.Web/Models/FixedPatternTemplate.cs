using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class FixedPatternTemplate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int FixedPatternID { get; set; }
        public FixedPattern FixedPattern { get; set; }

        public int CoverageID { get; set; }
        public Coverage Coverage { get; set; }

        [Include]
        [Composition]
        public ICollection<FPTemplateRow> FPTemplateRows;
    }
}