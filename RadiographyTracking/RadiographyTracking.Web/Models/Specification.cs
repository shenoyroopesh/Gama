using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class Specification
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static Specification getSpecification(string specification, RadiographyContext ctx)
        {
            return ctx.Specifications.FirstOrDefault(p => p.Value.ToUpper() == specification.ToUpper());
        }
    }
}