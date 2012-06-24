using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    
    public class FilmStockReportRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public DateTime Date { get; set; }

        [NotMapped]
        public float OpeningStock { get; set; }

        [NotMapped]
        public float SentToHO { get; set; }

        [NotMapped]
        public float Consumed { get; set; }

        [NotMapped]
        public float ReceivedFromHO { get; set; }

        [NotMapped]
        public float ClosingStock { get; set; }
    }
}