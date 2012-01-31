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

namespace RadiographyTracking.Web.Models
{
    /// <summary>
    /// Extensions to the <see cref="Energy"/> class.
    /// </summary>
    public partial class Shift
    {
        public override String ToString()
        {
            return this.Value;
        }
    }
}
