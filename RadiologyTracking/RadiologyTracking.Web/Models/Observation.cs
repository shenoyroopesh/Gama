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
