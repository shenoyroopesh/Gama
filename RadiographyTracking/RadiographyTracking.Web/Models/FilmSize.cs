using System;
using System.ComponentModel.DataAnnotations;

namespace RadiographyTracking.Web.Models
{
    public class FilmSize
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public float Width { get; set; }
        public float Length { get; set; }

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

        public float Area
        {
            get
            {
                return Width * Length;
            }

// ReSharper disable ValueParameterNotUsed
            private set
// ReSharper restore ValueParameterNotUsed
            {
                
            }
        }
    }
}
