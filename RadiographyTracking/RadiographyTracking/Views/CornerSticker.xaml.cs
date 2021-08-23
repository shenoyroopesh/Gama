using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RadiographyTracking.Web.Services;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Browser;

namespace RadiographyTracking.Views
{
    public partial class CornerSticker : BaseCRUDView
    {
        RadiographyContext ctx;

        public CornerSticker()
        {
            InitializeComponent();
        }

        private void FetchOperation(object sender, RoutedEventArgs e)
        {
            busyIndicator.IsBusy = true;

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

            Coverage coverage = (Coverage)cmbCoverage.SelectedItem;

            Uri reportURI = new Uri(string.Format(appRoot + "DummyAddressStickerReportGenerate.aspx?TEMPLATE_NAME={0}&FP_NO={1}&COVERAGE_ID={2}&RT_NO={3}&CELL_NO={4}",
                                                                       "AddressLabels_Dummy.docx", txtFPNo.Text, coverage.ID,txtRTNo.Text,cellNo.Text),
                                                        UriKind.Absolute);

            HtmlPage.Window.Navigate(reportURI, "_blank");
            busyIndicator.IsBusy = false;
        }

        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;

            var report = ((LoadOperation<FixedPatternTemplate>)sender).Entities;

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

            //Uri reportURI = new Uri(string.Format(appRoot + "AddressStickerReportGenerate.aspx?TEMPLATE_NAME={0}&REPORT_NUMBER={1}&CELL_NO={2}&REPORT_ID={3}",
            //                                                        cmbAddressStickerTemplates.SelectedValue,
            //                                                        this.ReportNo,
            //                                                        cellNo,
            //                                                        this.RGReportID),
            //                                        UriKind.Absolute);

            //HtmlPage.Window.Navigate(reportURI, "_blank");

            busyIndicator.IsBusy = false;

        }
    }
}
