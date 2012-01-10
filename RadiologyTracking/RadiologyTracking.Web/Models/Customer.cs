﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class Customer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String CustomerName { get; set; }

        public int FoundryID { get; set; }
        /// <summary>
        /// Every Customer belongs to a particular foundry, this particular note will tell which foundry
        /// </summary>
        public Foundry Foundry { get; set; }
        public ICollection<FixedPattern> FixedPatterns;
    }
}
