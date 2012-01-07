using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiologyTracking.Web.Models
{
    public class RGReportSource
    {
        public int ID { get; set; }

        public int EnergyID { get; set; }
        public Energy Energy { get; set; }

        public int RGReportID { get; set; }
        public RGReport RGReport { get; set; }
    }
}