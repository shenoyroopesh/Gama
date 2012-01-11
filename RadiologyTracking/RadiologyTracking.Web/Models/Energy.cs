using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    /// <summary>
    /// Represents the Radioactive Energy that can be used for taking the exposure for the prints
    /// </summary>
    [Serializable]
    public class Energy
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage="Name is required")]
        public String Name { get; set; }
    }
}