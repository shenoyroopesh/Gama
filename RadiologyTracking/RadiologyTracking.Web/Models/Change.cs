using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiologyTracking.Web.Models
{
    public class Change
    {        
        public String What { get; set; }
        public String Where { get; set; }
        public DateTime When { get; set; }
        public String FromValue { get; set; }
        public String ToValue { get; set; }
        public User ByWhom { get; set; }
        public String Why { get; set; }
    }
}