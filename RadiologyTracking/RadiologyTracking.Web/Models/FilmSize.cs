using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class FilmSize
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
