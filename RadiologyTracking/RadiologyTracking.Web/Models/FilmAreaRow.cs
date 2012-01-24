using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class FilmAreaRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String FilmSize { get; set; }

        [NotMapped]
        public int Total { get; set; }

        [NotMapped]
        public int RT { get; set; }

        [NotMapped]
        public Guid ShiftWisePerformanceRowID { get; set; }
    }
}