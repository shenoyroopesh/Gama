using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace RadiologyTracking.Web.Models
{
    public class ShiftWisePerformanceRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String Technicians { get; set; }

        [NotMapped]
        public String Date { get; set; }
        [NotMapped]
        public String Shift { get; set; }

        [NotMapped]
        public int TotalFilmsTaken { get; set; }
        [NotMapped]
        public int TotalRetakes { get; set; }
        [NotMapped]
        public double RTPercent { get; set; }
        [NotMapped]
        public double RTPercentByArea { get; set; }

        [Include]
        [Association("FilmAreaRow", "ID", "ShiftWisePerformanceRowID")]
        public IEnumerable<FilmAreaRow> FilmAreaRows { get; set; }
    }
}