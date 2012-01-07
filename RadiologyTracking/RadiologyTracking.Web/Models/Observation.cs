using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiologyTracking.Web.Models
{
    public class Observation
    {
        public int ID { get; set; }
        public Defect Defect { get; set; }
        public int Level { get; set; }
    }
}
