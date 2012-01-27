using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server;

namespace RadiologyTracking.Web.Models
{
    public class ThicknessRangeForEnergy
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public double ThicknessFrom { get; set; }
        public double ThicknessTo { get; set; }

        public int EnergyID { get; set; }

        [Include]
        public Energy Energy { get; set; }
    }
}