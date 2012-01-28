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

namespace RadiologyTracking.Web.Models
{
    /// <summary>
    /// Extensions to the <see cref="FilmSize"/> class.
    /// </summary>
    public partial class FilmSize
    {
        public override String ToString()
        {
            return this.Name;
        }
    }
}
