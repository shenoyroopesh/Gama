using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FixedPatternTemplate
    {
        public FixedPatternTemplate()
        {
            this.FPTemplateRows = new List<FPTemplateRow>();
        }


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public int? FixedPatternID { get; set; }
        public FixedPattern FixedPattern { get; set; }

        public int CoverageID { get; set; }
        public Coverage Coverage { get; set; }

        [Include]
        public ICollection<FPTemplateRow> FPTemplateRows { get; set; }
    }
}