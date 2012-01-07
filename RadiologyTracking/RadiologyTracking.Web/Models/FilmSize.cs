using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiologyTracking.Web.Models
{
    public class FilmSize
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int Area
        {
            get {
                return Width * Height;
            }
        }
    }
}
