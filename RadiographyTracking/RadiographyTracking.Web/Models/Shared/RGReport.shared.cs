using System;
using System.ComponentModel;
using System.Linq;

namespace RadiographyTracking.Web.Models
{
    public partial class RGReport : INotifyPropertyChanged
    {
        private string evaluationAsPer;
        public string EvaluationAsPer
        {
            get
            {

                if (RGReportRows == null)
                    return string.Empty;
                else
                {
                    try
                    {
                        var range1 = 
                            RGReportRows.Count(
                                p =>
                                Convert.ToInt32(p.ThicknessRange) >= 0 && Convert.ToInt32(p.ThicknessRange) <= 50);
                        var range2 =
                            RGReportRows.Count(
                                p =>
                                Convert.ToInt32(p.ThicknessRange) >= 51 && Convert.ToInt32(p.ThicknessRange) <= 114);
                        var range3 = 
                        RGReportRows.Count(
                            p =>
                            Convert.ToInt32(p.ThicknessRange) >= 115 && Convert.ToInt32(p.ThicknessRange) <= 305);

                        var eval = "ASTM";
                        if (range1 > 0)
                            eval = eval + " E446";
                        if (range2 > 0)
                            eval = eval == "ASTM" ? eval + " E186" : eval + " / E186 ";
                        if (range3 > 0)
                            eval = eval == "ASTM" ? eval + " E280" : eval + " / E280 ";

                        return eval == "ASTM" ? string.Empty : eval;
                    }
                    catch
                    {
                        return string.Empty;
                    }
                }
            }
            set
            {
                if (evaluationAsPer != value)
                {
                    evaluationAsPer = value;
                    this.RaisePropertyChanged("EvaluationAsPer");

                }
            }


        }

        
    }

        
}
