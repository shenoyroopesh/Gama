﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiologyTracking.Web.Models
{
    public class RGReportRow
    {
        public int SlNo { get; set; }
        public String Location { get; set; }
        public int Thickness { get; set; }
        public Energy Energy { get; set; }
        public int SFD { get; set; }
        public String Designation { get; set; }
        public String Sensitivity { get; set; }
        public String Density { get; set; }
        public FilmSize FilmSize { get; set; }
        public Observation[] Observations { get; set; }
        public Remark Remark { get; set; }
        public Technician Technician { get; set; }
        public Welder Welder { get; set; }

        public RGReport RGReport { get; set; }
    }
}
