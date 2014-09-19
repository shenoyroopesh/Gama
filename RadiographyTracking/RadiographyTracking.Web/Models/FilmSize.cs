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

        public float WidthInCms { get; set; }
        public float LengthInCms { get; set; }

        /// <summary>
        /// Returns the name in the format Height X Width
        /// </summary>
        public String NameInCms
        {
            get
            {
                if (LengthInCms > WidthInCms)
                    return String.Concat(LengthInCms.ToString(),
                    "X",
                    WidthInCms.ToString());
                else
                    return String.Concat(WidthInCms.ToString(),
                    "X",
                    LengthInCms.ToString());
            }
        }

        public float AreaInCms
        {
            get
            {
                return WidthInCms * LengthInCms;
            }

            // ReSharper disable ValueParameterNotUsed
            private set
            // ReSharper restore ValueParameterNotUsed
            {

            }
        }
    }
}
