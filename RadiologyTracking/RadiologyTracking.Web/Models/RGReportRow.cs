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

        [Include, Composition]
        public ICollection<Observation> Observations { get; set; }
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

        /// <summary>
        /// This property is only for setting observations for the row using a comma separated string of observations and not
        /// for anything else. This won't be saved in the database
        /// </summary>
        [NotMapped]
        public String ObservationsText
        {
            get
            {
                return string.Join(",", this.Observations.Select(p=>p.ToString()));
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
                        Observation observation = new Observation(o);
                        newObservations.Add(observation);
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

    }
}
