using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class AcceptanceAsPer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static AcceptanceAsPer getAcceptanceAsPer(string acceptanceAsPer, RadiographyContext ctx)
        {
            return ctx.AcceptanceAsPers.FirstOrDefault(p => p.Value.ToUpper() == acceptanceAsPer.ToUpper());
        }
    }
}