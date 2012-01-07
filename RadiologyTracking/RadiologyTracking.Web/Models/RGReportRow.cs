using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiologyTracking.Web.Models
{
    public class RGReportRow
    {
        public int ID { get; set; }
        public int SlNo { get; set; }
        public String Location { get; set; }
        public int Thickness { get; set; }

        public int EnergyID { get; set; }
        public Energy Energy { get; set; }

        public int SFD { get; set; }
        public String Designation { get; set; }
        public String Sensitivity { get; set; }
        public String Density { get; set; }

        public int FilmSizeID { get; set; }
        public FilmSize FilmSize { get; set; }

        public ICollection<Observation> Observations { get; set; }
        public Remark Remark { get; set; }

        public int TechnicianID { get; set; }
        public Technician Technician { get; set; }

        public int WelderID { get; set; }
        public Welder Welder { get; set; }

        public int RGReportID { get; set; }
        public RGReport RGReport { get; set; }
    }
}
