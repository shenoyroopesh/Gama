﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class Foundry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String FoundryName { get; set; }
        public String Address { get; set; }

        [Required]
        public String ReportNumberPrefix { get; set; }
        public ICollection<Customer> Customers { get; set; }

        public ICollection<Period> Periods { get; set; } 

        public Period CurrentPeriod
        {
            get { return Periods.Count == 0? null : Periods.First(p => p.StartDate == Periods.Min(q => q.StartDate)); }
        }

        public DateTime NextResetDate { get; set; }

        public string ReportTemplate { get; set; }

        /// <summary>
        /// Gets the next report number for this foundry
        /// </summary>
        /// <param name="ctx">Context under which to give the report number</param>
        /// <returns></returns>
        public String getNextReportNumber(RadiographyContext ctx)
        {
            //fetch immediately from the database, otherwise convert.toint32 will fail
            var reports = ctx.RGReports.Where(p => p.ReportNo.StartsWith(this.ReportNumberPrefix)).ToList();
            var lastNumber = !reports.Any() ? 0 : reports.Max(p => Convert.ToInt32(p.ReportNo.Replace(ReportNumberPrefix, "")));
            return String.Concat(ReportNumberPrefix, Convert.ToString(lastNumber + 1));
        }
    }
}