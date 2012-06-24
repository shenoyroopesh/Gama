using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FilmTransaction
    {
        /// <summary>
        /// Constructor - initialize the transaction date to today for convenience
        /// </summary>
        public FilmTransaction()
        {
            this.Date = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public int FoundryID { get; set; }

        public Foundry Foundry { get; set; }
        public String ChallanNo { get; set; }


        public int DirectionID { get; set; }

        public Direction Direction { get; set; }

        public float Area { get; set; }
    }
}