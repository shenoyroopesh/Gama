using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Linq;

namespace RadiographyTracking.Web.Models
{
    public partial class RGReportRow
    {
        public string ThicknessRangeUI
        {
            get
            {
                return this.ThicknessRange;
            }

            set
            {
                try
                {
                    this.Thickness = Convert.ToInt32(value.Split('-').ToList().Select(p => int.Parse(p.Trim())).Max());
                    this.ThicknessRange = value;
                }
                catch
                {
                    throw new ArgumentException("Enter in the proper format - for e.g. 10-20");
                }
            }
        }

        static string[] _techniques = new string[] { "SWSI", "DWSI", "SI" };

        public string[] TechniqueSource
        {
            get 
            {
                return _techniques;
            }
        }

        public string TechniqueText
        {
            get 
            {
                return String.IsNullOrEmpty(this.Technique) ? " " : this.Technique;
            }

            set
            {
                this.Technique = value;
            }
        }
    }
}
