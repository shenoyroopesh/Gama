using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class RetakeReason
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static RetakeReason getRetakeReasons(string retakeReason, RadiographyContext ctx)
        {
            return ctx.RetakeReasons.FirstOrDefault(p => p.Value.ToUpper() == retakeReason.ToUpper());
        }
    }
}