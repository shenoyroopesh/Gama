using System;
using System.ComponentModel;
using System.Linq;

namespace RadiographyTracking.Web.Models
{
    public partial class RGReport
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
                        int range1=0, range2=0, range3=0;

                        foreach (var row in RGReportRows)
                        {
                            var thicknessValue = Convert.ToInt32(row.ThicknessRange.Split('-').ToList().Select(k => int.Parse(k.Trim())).Average());
                            if (thicknessValue >= 0 && thicknessValue <= 50)
                                range1++;
                            if (thicknessValue >= 51 && thicknessValue <= 114)
                                range2++;
                            if (thicknessValue >= 115 && thicknessValue <= 305)
                                range3++;
                        }
                   

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
            set { evaluationAsPer = value; }

        }
       
    }

        
}
