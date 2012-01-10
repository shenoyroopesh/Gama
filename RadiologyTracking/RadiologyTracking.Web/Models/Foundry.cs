using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class Foundry
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String FoundryName { get; set; }
        public String Address { get; set; }
        public String ReportNumberPrefix { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}