using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class RGReportRow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SlNo { get; set; }
        public String Location { get; set; }
        public String Segment { get; set; }
        public int Thickness { get; set; }

        public int EnergyID { get; set; }
        public Energy Energy { get; set; }

        public int SFD { get; set; }
        public String Designation { get; set; }
        public String Sensitivity { get; set; }
        public String Density { get; set; }

        public int FilmSizeID { get; set; }
        public FilmSize FilmSize { get; set; }

        public String Observations { get; set; }

        public int? RemarkID { get; set; }
        public Remark Remark { get; set; }

        public int? TechnicianID { get; set; }
        public Technician Technician { get; set; }

        public int? WelderID { get; set; }
        public Welder Welder { get; set; }

        public int RGReportID { get; set; }
        public RGReport RGReport { get; set; }

        /// <summary>
        /// This will determine the cause for this row in the first place - necessary to track this separately from remark
        /// 
        /// For eg, in a first report, the cause for the row will be FRESH but the remark could be REPAIR. In this case, the next report will have a corresponding row 
        /// whose cause is REPAIR which can again have different REMARK such as REPAIR or RESHOOT or ACCEPTABLE
        /// </summary>
        public RGReportRowType RowType { get; set; }

        public int RowTypeID { get; set; }

        [NotMapped]
        public String FilmSizeString
        {
            get
            {
                if (this.FilmSizeID == 0) return " ";
                //TODO: see if context can be injected instead of using like this
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var filmSizes = ctx.FilmSizes.Where(p => p.ID == this.FilmSizeID);
                    if (filmSizes.Count() > 0)
                        return filmSizes.First().Name;
                    else
                        return " ";
                }
            }
            set
            {
                int height, width;
                try
                {
                    String[] dimensions = value.Split('X');
                    height = Convert.ToInt32(dimensions[0]);
                    width = Convert.ToInt32(dimensions[1]);
                }
                catch
                {
                    return;
                }

                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var filmsizes = ctx.FilmSizes.Where(p => p.Height == height && p.Width == width);
                    if (filmsizes.Count() > 0)
                    {
                        this.FilmSize = filmsizes.First();
                        this.FilmSizeID = filmsizes.First().ID;
                    }
                }
            }
        }

        /// <summary>
        /// Only for setting or seeing Remark by using a string value
        /// </summary>
        [NotMapped]
        public string RemarkText
        {
            get
            {
                if (this.RemarkID == 0 || this.RemarkID == null) return " ";
                //TODO: see if context can be injected instead of using like this
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var remarks = ctx.Remarks.Where(p => p.ID == this.RemarkID);
                    if (remarks.Count() > 0)
                        return remarks.First().Value;
                    else
                        return " ";
                }
            }
            set
            {                
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    this.Remark = Remark.getRemark(value, ctx);
                    if (this.Remark != null) this.RemarkID = this.Remark.ID;
                }
            }
        }

        /// <summary>
        /// Only for setting or seeing Welder by using a string value
        /// </summary>
        [NotMapped]
        public string WelderText
        {
            get
            {
                if (this.WelderID == 0 || this.WelderID == null) return " ";
                //TODO: see if context can be injected instead of using like this
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var welders = ctx.Welders.Where(p => p.ID == this.WelderID);
                    if (welders.Count() > 0)
                        return welders.First().Name;
                    else
                        return " ";
                }
            }
            set
            {
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    this.Welder = Welder.getWelder(value, ctx);
                    if (this.Welder != null) this.WelderID = this.Welder.ID;
                }
            }
        }

        /// <summary>
        /// Only for setting or seeing Technician by using a string value
        /// </summary>
        [NotMapped]
        public string TechnicianText
        {
            get
            {
                if (this.TechnicianID == 0 || this.TechnicianID == null) return " ";
                //TODO: see if context can be injected instead of using like this
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var technicians = ctx.Technicians.Where(p => p.ID == this.TechnicianID);
                    if (technicians.Count() > 0)
                        return technicians.First().Name;
                    else
                        return " ";
                }
            }
            set
            {
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    this.Technician = Technician.getTechnician(value, ctx);
                    if (this.Technician != null) this.TechnicianID = this.Technician.ID;
                }
            }
        }
    }
}
