using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class Observation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// Default constructor, doesn't do much
        /// </summary>
        public Observation()
        {
        }

        /// <summary>
        /// Constructor with a string, splits the input into Defect and level
        /// </summary>
        /// <param name="observation">String value of observation</param>
        /// <param name="ctx">Database Context with reference which to create the object</param>
        public Observation(String observation, RadiologyContext ctx)
        {
            //if the string contains less or more than 2 characters throw an exception
            //we are assuming that the string contains only 2 characters, first character corresponds
            //to the Defect and the second character corresponds to the Level

            if (observation.Length != 2)
            {
                throw new ArgumentException("Observation should be only two characters");
            }

            int level;

            if (!Int32.TryParse(observation[1].ToString(), out level))
            {
                throw new ArgumentException("Level should be numeric");
            }

            //need to validate against existing defects
            var rows = ctx.Defects.Where(p => p.Code == observation[0].ToString());
            if (rows.Count() == 0)
            {
                throw new ArgumentException("Defect with code "+ observation[0].ToString() + "is not defined in the database");
            }
            this.Defect = rows.First();
            this.Level = level;
        }

        public int DefectID { get; set; }
        public Defect Defect { get; set; }

        public int Level { get; set; }

        public int RGReportRowID { get; set; }

        public override string ToString()
        {
            return String.Concat(Defect.Code, Level.ToString());
        }
       
    }
}
