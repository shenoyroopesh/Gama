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
        public int OpeningStock { get; set; }

        [NotMapped]
        public int SentToHO { get; set; }

        [NotMapped]
        public int Consumed { get; set; }

        [NotMapped]
        public int ReceivedFromHO { get; set; }

        [NotMapped]
        public int ClosingStock { get; set; }
    }
}