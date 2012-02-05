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
using System.Windows.Navigation;
using System.Windows.Browser;

namespace RadiographyTracking.Views
{
    public partial class RadiographyReports : BaseCRUDView
    {
        public RadiographyReports()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;

            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.RGDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.RGDomainDataSource; }
        }

        public override Type MainType
        {
            get { return typeof(RGReport); }
        }


        public void EditOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            App.RGReport = (RGReport)row.DataContext;
            Navigate("/EnterRadioGraphyReport");
        }

        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            App.RGReport = null;
            Navigate("/EnterRadioGraphyReport");
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            RGReport report = (RGReport)row.DataContext;
            if (!report.CanDelete)
            {
                MessageBox.Show("Can Delete only latest report for any RT");
                return;
            }

            base.DeleteOperation(sender, e);
        }


        public void PrintOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            RGReport report = (RGReport)row.DataContext;

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

            Uri reportURI = new Uri(string.Format(appRoot + "RGReportGenerate.aspx?ReportNo={0}", report.ReportNo), UriKind.Absolute);
            HtmlPage.Window.Navigate(reportURI, "_blank");
        }
    }
}
