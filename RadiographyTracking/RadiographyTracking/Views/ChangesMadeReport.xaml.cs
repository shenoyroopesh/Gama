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
using System.ComponentModel.DataAnnotations;
using Vagsons.Controls;
using System.Windows.Browser;

namespace RadiographyTracking.Views
{
    public partial class ChangesMadeReport : BaseCRUDView
    {
        public ChangesMadeReport()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.changesDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.changesDomainDataSource; }
        }

        public override Type MainType
        {
            get { return typeof(Change); }
        }

        private void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            var foundryName = (bool)chkAllFoundries.IsChecked ? null :
                                    (cmbFoundry.SelectedIndex == -1 ? null :
                                            (cmbFoundry.SelectedItem as Foundry).FoundryName);

            var fromDate = fromDatePicker.SelectedDate;
            var toDate = toDatePicker.SelectedDate;

            changesDomainDataSource.QueryParameters.Clear();
            changesDomainDataSource.QueryParameters.Add(new Parameter() { ParameterName = "foundryName", Value = foundryName });
            changesDomainDataSource.QueryParameters.Add(new Parameter() { ParameterName = "fromDate", Value = fromDate });
            changesDomainDataSource.QueryParameters.Add(new Parameter() { ParameterName = "toDate", Value = toDate });
            changesDomainDataSource.Load();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var foundryName = (bool)chkAllFoundries.IsChecked ? null : 
                        (cmbFoundry.SelectedIndex == -1? null: 
                                (cmbFoundry.SelectedItem as Foundry).FoundryName);

            var fromDate = fromDatePicker.SelectedDate;
            var toDate = toDatePicker.SelectedDate;

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

            Uri reportURI = new Uri(string.Format(appRoot + "ChangesReportGenerate.aspx?FOUNDRY_NAME={0}&FROM_DATE={1}&TO_DATE={2}",
                                                    (foundryName == null? "" : foundryName.ToString()),
                                                    (fromDate == null ? "" : ((DateTime)fromDate).ToString("MM/dd/yyyy")),
                                                    (toDate == null ? "" : ((DateTime)toDate).ToString("MM/dd/yyyy"))), UriKind.Absolute);

            HtmlPage.Window.Navigate(reportURI, "_blank");
        }
    }
}
