using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiologyTracking.Web.Models
{
    public class Customer
    {
        public String CustomerName { get; set; }

        /// <summary>
        /// Every Customer belongs to a particular foundry, this particular note will tell which foundry
        /// </summary>
        public String Foundry { get; set; }
    }
}
