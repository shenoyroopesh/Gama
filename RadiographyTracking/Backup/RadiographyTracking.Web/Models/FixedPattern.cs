using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FixedPattern
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String FPNo { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public String Description { get; set; }
    }
}