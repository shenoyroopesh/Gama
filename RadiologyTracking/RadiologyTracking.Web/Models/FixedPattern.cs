using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiologyTracking.Web.Models
{
    public class FixedPattern
    {
        public int ID { get; set; }
        public String FPNo { get; set; }
        public Customer Customer { get; set; }
        public String Description { get; set; }
    }
}