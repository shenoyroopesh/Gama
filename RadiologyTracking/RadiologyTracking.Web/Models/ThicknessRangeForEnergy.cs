using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiologyTracking.Web.Models
{
    public class ThicknessRangeForEnergy
    {
        public int ID { get; set; }
        public double ThicknessFrom { get; set; }
        public double ThicknessTo { get; set; }
        public Energy Energy { get; set; }
    }
}