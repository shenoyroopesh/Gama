using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RadiographyTracking.Web.Models
{
    public class AddressStickerRow
    {
        public RGReportRow AddressLabelCol1 { get; set; }
        public RGReportRow AddressLabelCol2 { get; set; }
    }

    public class DummyAddressStickerRow
    {
        public FPTemplateRow AddressLabelCol1 { get; set; }
        public FPTemplateRow AddressLabelCol2 { get; set; }
        public FPTemplateRow AddressLabelCol3 { get; set; }

        public string CoverageName { get; set; }
        public string FPNo { get; set; }
        public string RTNo { get; set; }
        public string LocationAndSegment { get; set; }
        public string HeatNo { get; set; }

    }
}