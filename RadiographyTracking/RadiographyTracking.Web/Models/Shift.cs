using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class Shift
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static Shift getShift(string value, RadiographyContext ctx)
        {
            return ctx.Shifts.First(p => p.Value == value);
        }
    }
}
