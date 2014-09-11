using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class Observation
    {
        public Observation()
        {
            IsChecked = false;   
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        [NotMapped]
        public bool IsChecked { get; set; }

        public static Observation getObservation(string observationReference, RadiographyContext ctx)
        {
            return ctx.Observations.FirstOrDefault(p => p.Value.ToUpper() == observationReference.ToUpper());
        }
    }
}