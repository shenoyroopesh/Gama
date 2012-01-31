using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FilmSize
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }

        /// <summary>
        /// Returns the name in the format Height X Width
        /// </summary>
        public String Name
        {
            get {
                return String.Concat(Width.ToString(), 
                                    "X", 
                                    Length.ToString()); 
            }
        }

        public int Area
        {
            get
            {
                return Width * Length;
            }

            private set
            {

            }
        }
    }
}
