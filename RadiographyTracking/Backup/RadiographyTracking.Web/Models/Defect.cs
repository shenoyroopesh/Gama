using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class Defect
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Code { get; set; }
        public String Name { get; set; }
    }
}
