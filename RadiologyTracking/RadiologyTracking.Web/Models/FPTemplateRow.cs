using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace RadiologyTracking.Web.Models
{
    public class FPTemplateRow
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int SlNo { get; set; }
        public String Location { get; set; }
        public String Segment { get; set; }
        public int Thickness { get; set; }
        public int SFD { get; set; }
        public String Designation { get; set; }
        public String Sensitivity { get; set; }
        public String Density { get; set; }

        public int FilmSizeID { get; set; }
        public FilmSize FilmSize { get; set; }

        [NotMapped]
        public String FilmSizeString
        {
            get
            {
                if(this.FilmSizeID == 0) return  " ";
                //TODO: see if context can be injected instead of using like this
                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var filmSizes = ctx.FilmSizes.Where(p => p.ID == this.FilmSizeID);
                    if (filmSizes.Count() > 0)
                        return filmSizes.First().Name;
                    else
                        return " ";
                }
            }
            set
            {
                int height, width;
                try
                {
                    String[] dimensions = value.Split('X');
                    height = Convert.ToInt32(dimensions[0]);
                    width = Convert.ToInt32(dimensions[1]);
                }
                catch
                {
                    return;
                }

                using (RadiologyContext ctx = new RadiologyContext())
                {
                    var filmsizes = ctx.FilmSizes.Where(p => p.Height == height && p.Width == width);
                    if (filmsizes.Count() > 0)
                    {
                        this.FilmSize = filmsizes.First();
                        this.FilmSizeID = filmsizes.First().ID;
                    }
                }
            }
        }

        public int FixedPatternTemplateID { get; set; }
        public FixedPatternTemplate FixedPatternTemplate { get; set; }
    }
}
