using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiologyTracking.Web.Models
{
    /// <summary>
    /// These are the possible causes for a row to appear in a radiology report. 
    /// </summary>
    public enum RGReportRowType
    {
        FRESH,
        REPAIR,
        RESHOOT,
        RETAKE
    }
}