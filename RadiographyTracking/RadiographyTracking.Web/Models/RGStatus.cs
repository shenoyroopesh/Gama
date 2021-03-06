﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RadiographyTracking.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class RGStatus
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public String Status { get; set; }

        public static RGStatus getStatus (string status, RadiographyContext ctx)
        {
            return ctx.RGStatuses.First(p => p.Status == status);
        }
    }
}
