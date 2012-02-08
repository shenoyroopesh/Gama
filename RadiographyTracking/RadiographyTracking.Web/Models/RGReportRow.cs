using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
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

        [Include]
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

        //note not doing composition here - instead just saving FPRowSLNo here which can be nullable
        public int? FPSLNo { get; set; }

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
                using (RadiographyContext ctx = new RadiographyContext())
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
                int length, width;
                try
                {
                    String[] dimensions = value.Split('X');
                    length = Convert.ToInt32(dimensions[0]);
                    width = Convert.ToInt32(dimensions[1]);
                }
                catch
                {
                    return;
                }

                using (RadiographyContext ctx = new RadiographyContext())
                {
                    var filmsizes = ctx.FilmSizes.Where(p => p.Length == length && p.Width == width);
                    if (filmsizes.Count() > 0)
                    {
                        this.FilmSizeID = filmsizes.First().ID;
                    }
                }
            }
        }

        /// <summary>
        /// Only for setting or seeing Energy by using a string value
        /// </summary>
        [NotMapped]
        public string EnergyText
        {
            get
            {
                if (this.EnergyID == 0) return " ";
                //TODO: see if context can be injected instead of using like this
                using (RadiographyContext ctx = new RadiographyContext())
                {
                    var remarks = ctx.Energies.Where(p => p.ID == this.EnergyID);
                    if (remarks.Count() > 0)
                        return remarks.First().Name;
                    else
                        return " ";
                }
            }
            set
            {
                using (RadiographyContext ctx = new RadiographyContext())
                {
                    try
                    {
                        this.EnergyID = Energy.getEnergyFromName(value, ctx).ID;
                    }
                    catch
                    {
                        //do nothing
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
                using (RadiographyContext ctx = new RadiographyContext())
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
                using (RadiographyContext ctx = new RadiographyContext())
                {
                    try
                    {
                        this.RemarkID = Remark.getRemark(value, ctx).ID;
                    }
                    catch
                    {
                        //do nothing
                    }
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
                using (RadiographyContext ctx = new RadiographyContext())
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
                using (RadiographyContext ctx = new RadiographyContext())
                {
                    try
                    {
                        this.WelderID = Welder.getWelder(value, ctx).ID;
                    }
                    catch
                    {
                        //do nothing
                    }
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
                using (RadiographyContext ctx = new RadiographyContext())
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
                using (RadiographyContext ctx = new RadiographyContext())
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

        /// <summary>
        /// Gets a combination of location and segment - for eg, if location is LN and segment is 1-2, then 
        /// this returns LN1-LN2
        /// </summary>
        [NotMapped]
        [Exclude]
        public string LocationAndSegment
        {
            get
            {
                //very rarely this should happen, but check none-the-less
                if (string.IsNullOrEmpty(this.Location) || string.IsNullOrEmpty(this.Segment))
                {
                    return "";
                }
                //if location is LN and segment is 1-2, return LN1-LN2
                var segments = this.Segment.Split('-');
                var joined = segments.Select(p => this.Location + p);
                return String.Join("-", joined);
            }
        }
    }
}