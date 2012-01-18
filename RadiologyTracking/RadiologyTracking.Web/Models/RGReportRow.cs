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
        public RGReportRow()
        {
            this.Observations = new List<Observation>();
        }

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

        [Include]
        public ICollection<Observation> Observations { get; set; }

        public int RemarkID { get; set; }
        public Remark Remark { get; set; }

        public int TechnicianID { get; set; }
        public Technician Technician { get; set; }

        public int WelderID { get; set; }
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
        /// This property is only for setting observations for the row using a comma separated string of observations and not
        /// for anything else. This won't be saved in the database
        /// </summary>
        [NotMapped]
        public String ObservationsText
        {
            get
            {
                return string.Join(",", this.Observations.Select(p => p.ToString()));
            }
            set
            {
                //break the text up and create a new observation object for each of the text. If any 
                // segment not parse well just ignore it

                List<Observation> newObservations = new List<Observation>();
                String[] obs = value.Split(',');
                foreach (var o in obs)
                {
                    try
                    {
                        using (RadiologyContext ctx = new RadiologyContext())
                        {
                            Observation observation = new Observation(o.Trim(), ctx);
                            newObservations.Add(observation);
                        }                        
                    }
                    catch (ArgumentException e)
                    {
                        continue;
                    }

                    //if all goes well, delete existing observations and add these ones
                    this.Observations.Clear();
                    newObservations.ForEach(p => this.Observations.Add(p));
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
                return this.Remark == null ? String.Empty : this.Remark.Value;
            }
            set
            {                
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    this.Remark = Remark.getRemark(value, ctx);
                }
            }
        }

    }
}
