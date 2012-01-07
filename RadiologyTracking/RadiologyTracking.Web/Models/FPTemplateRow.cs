using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiologyTracking.Web.Models
{
    public class FPTemplateRow
    {
        public int SlNo { get; set; }
        public String Location { get; set; }
        public int Thickness { get; set; }
        public int SFD { get; set; }
        public String Designation { get; set; }
        public String Sensitivity { get; set; }
        public String Density { get; set; }
        public FilmSize FilmSize { get; set; }
        public FixedPatternTemplate FixedPatternTemplate { get; set; }
    }
}
