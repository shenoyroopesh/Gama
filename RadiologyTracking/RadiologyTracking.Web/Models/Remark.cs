using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiologyTracking.Web.Utility;

namespace RadiologyTracking.Web.Models
{
    public enum Remark
    {
        [StringValue("ACC")]
        ACCEPTABLE,
        [StringValue("R")]
        REPAIR,
        [StringValue("RS")]
        RESHOOT,
        [StringValue("RT")]
        RETAKE
    }
}
