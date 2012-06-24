using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class Remark
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static Remark getRemark(string remark, RadiographyContext ctx)
        {
            return ctx.Remarks.FirstOrDefault(p => p.Value.ToUpper() == remark.ToUpper());
        }
    }
}
