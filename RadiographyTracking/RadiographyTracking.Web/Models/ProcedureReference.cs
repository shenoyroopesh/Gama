using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class ProcedureReference
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static ProcedureReference getProcedureReference(string procedureReference, RadiographyContext ctx)
        {
            return ctx.ProcedureReferences.FirstOrDefault(p => p.Value.ToUpper() == procedureReference.ToUpper());
        }
    }
}