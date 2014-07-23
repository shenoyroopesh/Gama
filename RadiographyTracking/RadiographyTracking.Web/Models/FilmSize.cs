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
            get
            {
                ///Change made by Praveen
                ///14th June, 2014
                ///According to Requirement #1 from Requirements - 12June2014

                //return String.Concat(Width.ToString(), 
                //                    "X", 
                //                    Length.ToString());
                if (Length > Width)
                    return String.Concat(Length.ToString(),
                    "X",
                    Width.ToString());
                else
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
