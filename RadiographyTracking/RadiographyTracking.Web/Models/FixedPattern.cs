﻿using System;
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

         [RegularExpression(@"^[A-Za-z]{2}[0-9]{2}(?:[A-Za-z0-9_@./#&+-]{0,4})?$", ErrorMessage =
            "FP No can be maximum of eight characters – First two must be alphabet. Next two must be numeral. Remaining four can be alphanumeric or special characters.")]
        public String FPNo { get; set; }

        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        public String Description { get; set; }
    }
}