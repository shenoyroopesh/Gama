using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiologyTracking.Web.Utility;

namespace RadiologyTracking.Web.Models
{
    public enum RGStatus
    {
        [StringValue("Pending")]
        PENDING,
        [StringValue("Complete")]
        COMPLETE
    }
}
