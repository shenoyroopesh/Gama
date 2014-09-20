using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class RetakeReasonReportRow
    {
        [Key]
        public Guid ID { get; set; }

        [NotMapped]
        public String FPNo { get; set; }

        [NotMapped]
        public string Coverage { get; set; }

        [NotMapped]
        public String RTNo { get; set; }

        [NotMapped]
        public String Location { get; set; }

        [NotMapped]
        public String Segment { get; set; }

        [NotMapped]
        public String RetakeReason { get; set; }

        [NotMapped]
        public String ReportNo { get; set; }

        [NotMapped]
        public DateTime ReportDate { get; set; }

        [NotMapped]
        public DateTime DateOfTest { get; set; }

        [NotMapped]
        public int? TechnicianID { get; set; }

        [NotMapped]
        public String UserName { get; set; }

        [NotMapped]
        public string TechnicianName
        {
            get
            {
                if (this.TechnicianID == 0 || this.TechnicianID == null) return " ";
                //TODO: see if context can be injected instead of using like this
                using (var ctx = new RadiographyContext())
                {
                    var technicians = ctx.Technicians.Where(p => p.ID == this.TechnicianID);
                    return technicians.Any() ? technicians.First().Name : " ";
                }
            }
            set
            {
                using (var ctx = new RadiographyContext())
                {
                    try
                    {
                        this.TechnicianID = Technician.getTechnician(value, ctx).ID;
                    }
                    catch
                    {
                        //do nothing
                    }
                }
            }
        }

    }
}