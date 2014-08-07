﻿using System;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.ComponentModel.DataAnnotations;
using RadiographyTracking.Web.Utility;

namespace RadiographyTracking.Web.Models
{
    public class FinalRTReportRow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [NotMapped]
        public int SlNo { get; set; }
        [NotMapped]
        public String Location { get; set; }
        [NotMapped]
        public String Segment { get; set; }
        [NotMapped]
        public int Thickness { get; set; }

        [NotMapped]
        public string ThicknessRange { get; set; }

        public int EnergyID { get; set; }
        public Energy Energy { get; set; }
        [NotMapped]
        public int SFD { get; set; }
        [NotMapped]
        public String Designation { get; set; }
        [NotMapped]
        public String Sensitivity { get; set; }
        [NotMapped]
        public String Density { get; set; }
        
        public int FilmSizeID { get; set; }
        public FilmSize FilmSize { get; set; }

        [NotMapped]
        public int FilmCount { get; set; }


        private string _observations;

        //ensure that the findings and classifications are calculated as and when observations are updated
        [NotMapped]
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

        [NotMapped]
        public int? RemarkID { get; set; }
        [NotMapped]
        public Remark Remark { get; set; }
        [NotMapped]
        public int? TechnicianID { get; set; }
        [NotMapped]
        public Technician Technician { get; set; }
        [NotMapped]
        public int? WelderID { get; set; }
        [NotMapped]
        public Welder Welder { get; set; }
        [NotMapped]
        public int FinalRTReportID { get; set; }
        [NotMapped]
        public int? RetakeReasonID { get; set; }
        [NotMapped]
        public RetakeReason RetakeReason { get; set; }


        /// <summary>
        /// This will determine the cause for this row in the first place - necessary to track this separately from remark
        /// 
        /// For eg, in a first report, the cause for the row will be FRESH but the remark could be REPAIR. In this case, the next report will have a corresponding row 
        /// whose cause is REPAIR which can again have different REMARK such as REPAIR or RESHOOT or ACCEPTABLE
        /// </summary>
        [NotMapped]
        public RGReportRowType RowType { get; set; }
        [NotMapped]
        public int RowTypeID { get; set; }

        [NotMapped]
        public String FilmSizeString
        {
            get;
            set;
        }

        [NotMapped]
        public string FilmSizeWithCount
        {
            get;
            set;
        }

        /// <summary>
        /// Only for setting or seeing Remark by using a string value
        /// </summary>
        [NotMapped]
        public string RemarkText
        {
            get;
            set;
        }

        /// <summary>
        /// Only for setting or seeing Welder by using a string value
        /// </summary>
        [NotMapped]
        public string WelderText
        {
            get;
            set;
        }

        /// <summary>
        /// Only for setting or seeing Technician by using a string value
        /// </summary>
        [NotMapped]
        public string TechnicianText
        {
            get;
            set;
        }

        /// <summary>
        /// Technique used for this measurement
        /// </summary>
        [NotMapped]
        public string Technique { get; set; }

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
                if (string.IsNullOrEmpty(Location) || string.IsNullOrEmpty(Segment))
                    return "";
                
                //if location is LN and segment is 1-2, return LN1-LN2
                var segments = Segment.Split('-');
                var joined = segments.Select(p => Location + p);
                return String.Join("-", joined);
            }
        }

        [NotMapped]
        [Exclude]
        public float FilmArea
        {
            get { return FilmSize == null ? 0 : FilmSize.Area * FilmCount; }
        }

        /// <summary>
        /// Only for setting or seeing Retake Reason by using a string value
        /// </summary>
        [NotMapped]
        public string RetakeReasonText
        {
            get;
            set;
        }
    }
}