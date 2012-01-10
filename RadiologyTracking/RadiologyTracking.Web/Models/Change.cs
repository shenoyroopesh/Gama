using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class Change
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String What { get; set; }
        public String Where { get; set; }
        public DateTime When { get; set; }
        public String FromValue { get; set; }
        public String ToValue { get; set; }

        //TODO: check out whether this can be made user id later
        public String ByWhom { get; set; }

        public String Why { get; set; }
    }
}