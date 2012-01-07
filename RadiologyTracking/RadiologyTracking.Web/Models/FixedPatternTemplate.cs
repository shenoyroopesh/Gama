using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiologyTracking.Web.Models
{
    public class FixedPatternTemplate
    {
        public FixedPattern FixedPattern { get; set; }
        public Coverage Coverage { get; set; }
        public ICollection<FPTemplateRow> FPTemplateRows;
    }
}