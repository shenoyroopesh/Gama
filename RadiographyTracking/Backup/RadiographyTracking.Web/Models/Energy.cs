using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace RadiographyTracking.Web.Models
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


        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Get Energy for a particular Thickness by referencing the thickness-energy mapping
        /// </summary>
        /// <param name="thickness"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static Energy getEnergyForThickness(int thickness, RadiographyContext ctx)
        {
            return ctx.ThicknessRangesForEnergy.Include(p => p.Energy).First(p => p.ThicknessFrom <= thickness
                                                    && p.ThicknessTo >= thickness)
                                                    .Energy;
        }


        public static Energy getEnergyFromName(String name, RadiographyContext ctx)
        {
            return ctx.Energies.Where(p => p.Name == name).FirstOrDefault();
        }
    }
}