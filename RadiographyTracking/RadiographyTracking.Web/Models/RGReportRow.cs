using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;
using RadiographyTracking.Web.Utility;

namespace RadiographyTracking.Web.Models
{
    public class RGReportRow
    {
        public RGReportRow()
        {
            this.Technique = "SWSI";
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SlNo { get; set; }
        public String Location { get; set; }
        public String Segment { get; set; }

        /// <summary>
        /// This captures exactly what is entered in the UI
        /// Can only be numbers and dashes - like 88-90 for eg. 
        /// UI - level validation for this
        /// </summary>
        public string ThicknessRange { get; set; }

        /// <summary>
        /// This is derived from what is entered in the UI
        /// </summary>
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

        //added by Roopesh: 26-May-2012 - need to allow multiple films in a row
        public int FilmCount { get; set; }

        private string _observations;

        //ensure that the findings and classifications are calculated as and when observations are updated
        public String Observations
        {
            get { return _observations; }
            set
            {
                _observations = value;
                var split = _observations.SplitObservation();
                Findings = split.Item1;
                Classifications = split.Item2;
            }
        }

        //observations split into finding and classification
        [NotMapped]
        public String Findings { get; set; }

        [NotMapped]
        public string Classifications { get; set; }


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

        //This will be shown only in the address stickers, not in any of the casting report printouts. 
        public string Technique { get; set; }

        /// <summary>
        /// This will determine the cause for this row in the first place - necessary to track this separately from remark
        /// 
        /// For eg, in a first report, the cause for the row will be FRESH but the remark could be REPAIR. In this case, the next report will have a corresponding row 
        /// whose cause is REPAIR which can again have different REMARK such as REPAIR or RESHOOT or ACCEPTABLE
        /// </summary>
        public RGReportRowType RowType { get; set; }

        public int RowTypeID { get; set; }

        public int? RetakeReasonID { get; set; }
        public RetakeReason RetakeReason { get; set; }

        [NotMapped]
        public string FilmSizeWithCount
        {
            get
            {

                string FilmSizeWithCount = this.FilmSizeString + ((this.FilmCount > 1) ? ("X" + this.FilmCount.ToString()) : String.Empty);
                    //to show as 8X9X2 if there are two films of sizes 8X9
                return FilmSizeWithCount;
            }
        }

        [NotMapped]
        public String FilmSizeString
        {
            get
            {
                if (this.FilmSizeID == 0) return " ";
                //TODO: see if context can be injected instead of using like this
                using (var ctx = new RadiographyContext())
                {
                    var filmSizes = ctx.FilmSizes.Where(p => p.ID == this.FilmSizeID);
                    if (filmSizes.Any())
                        return filmSizes.First().Name;
                    else
                        return " ";
                }
            }
            set
            {
                float length, width;
                try
                {
                    var dimensions = value.Split('X');
                    length = float.Parse(dimensions[0]);
                    width = float.Parse(dimensions[1]);
                }
                catch
                {
                    return;
                }

                using (var ctx = new RadiographyContext())
                {
                    var filmsizes = ctx.FilmSizes.Where(p => p.Length == length && p.Width == width);
                    if (filmsizes.Any())
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
                using (var ctx = new RadiographyContext())
                {
                    var remarks = ctx.Energies.Where(p => p.ID == this.EnergyID);
                    return remarks.Any() ? remarks.First().Name : " ";
                }
            }
            set
            {
                using (var ctx = new RadiographyContext())
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
                using (var ctx = new RadiographyContext())
                {
                    var remarks = ctx.Remarks.Where(p => p.ID == this.RemarkID);
                    return remarks.Any() ? remarks.First().Value : " ";
                }
            }
            set
            {                
                using (var ctx = new RadiographyContext())
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
                using (var ctx = new RadiographyContext())
                {
                    var welders = ctx.Welders.Where(p => p.ID == this.WelderID);
                    return welders.Any() ? welders.First().Name : " ";
                }
            }
            set
            {
                using (var ctx = new RadiographyContext())
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

        /// <summary>
        /// Gets a combination of location and segment - for eg, if location is LN and segment is 1-2, then 
        /// this returns LN1-LN2
        /// </summary>
        [NotMapped]
        //[Exclude]
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
            set {  }
        }

        [NotMapped]
        [Exclude]
        public float FilmArea
        {
            get { return FilmSize == null ? 0 : FilmSize.Area*FilmCount; }
        }

        /// <summary>
        /// Only for setting or seeing Remark by using a string value
        /// </summary>
        [NotMapped]
        public string RetakeReasonText
        {
            get
            {
                if (this.RetakeReasonID == 0 || this.RetakeReasonID == null) return " ";
                //TODO: see if context can be injected instead of using like this
                using (var ctx = new RadiographyContext())
                {
                    var retakeReasons = ctx.RetakeReasons.Where(p => p.ID == this.RetakeReasonID);
                    return retakeReasons.Any() ? retakeReasons.First().Value : " ";
                }
            }
            set
            {
                using (var ctx = new RadiographyContext())
                {
                    try
                    {
                        this.RetakeReasonID = RetakeReason.getRetakeReasons(value, ctx).ID;
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