using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiologyTracking.Web.Utility;

namespace RadiologyTracking.Web.Models
{
    public enum Direction
    {
        [StringValue("Sent To HO")]
        SENT_TO_HO,
        [StringValue("Received From HO")]
        RECEIVED_FROM_HO
    }
}
