using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    /// <summary>
    /// These are the possible causes for a row to appear in a radiology report. 
    /// </summary>
    public class RGReportRowType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Value { get; set; }

        public static RGReportRowType getRowType(string rowType, RadiologyContext ctx)
        {
            return ctx.RGReportRowTypes.First(p => p.Value == rowType);
        }
    }
}