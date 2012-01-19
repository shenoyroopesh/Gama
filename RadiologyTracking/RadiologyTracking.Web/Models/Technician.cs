using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class Technician
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String Name { get; set; }

        public static Technician getTechnician(string name, RadiologyContext ctx)
        {
            return ctx.Technicians.FirstOrDefault(p => p.Name.ToUpper() == name.ToUpper());
        }
    }
}
