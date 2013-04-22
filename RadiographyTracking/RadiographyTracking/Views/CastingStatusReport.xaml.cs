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
using System.Threading;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;
using System.Collections;
using RadiographyTracking.Controls;
using System.Windows.Data;


namespace RadiographyTracking.Views
{
    public partial class CastingStatusReport : BaseCRUDView
    {        
        DataTable reportTable;
        RadiographyContext ctx;

        public CastingStatusReport()
            : base()
        {
            InitializeComponent();
           // fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            //toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiographyContext();
            busyIndicator.IsBusy = true;

            int foundryId = cmbFoundry.SelectedIndex == -1 ? -1 : ((Foundry)cmbFoundry.SelectedItem).ID;
            ctx.Load(ctx.GetRTStatusQuery(foundryId, (DateTime?)fromDatePicker.SelectedDate,
                (DateTime?)toDatePicker.SelectedDate, txtRTNo.Text, txtHeatNo.Text,txtFPNo.Text,txtCoverage.Text)).Completed += loadCompleted;            
        }

        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;
            var report = ((LoadOperation<RTStatusReportRow>)sender).Entities;
            reportGrid.ItemsSource = report;
            busyIndicator.IsBusy = false;

        }

        private static void AddTextColumn(DataTable reportTable, String columnName, String caption)
        {
            DataColumn dc = new DataColumn(columnName);
            dc.Caption = caption;
            dc.ReadOnly = true;
            dc.DataType = typeof(String);
            dc.AllowResize = true;
            dc.AllowSort = false;
            dc.AllowReorder = false;
            reportTable.Columns.Add(dc);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            reportGrid.Export("Roopesh", "Gama", "", 0);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                    
            btnFetch.IsEnabled = ValueChanged();              
        }

        private void grdDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            App.FinalReport = new FinalRTReport() { RTNo = ((RTStatusReportRow)row.DataContext).RTNo };
            Navigate("/FinalRadiographyReport");
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            btnFetch.IsEnabled = ValueChanged();
        }

        public bool ValueChanged()
        {
            return (!(cmbFoundry.SelectedIndex == -1 || String.IsNullOrEmpty(fromDatePicker.Text) ||
                     String.IsNullOrEmpty(toDatePicker.Text))
                   || !(cmbFoundry.SelectedIndex == -1 || (String.IsNullOrEmpty(txtRTNo.Text) &&
                                                           String.IsNullOrEmpty(txtHeatNo.Text))))
             && ((String.IsNullOrEmpty(txtFPNo.Text) && String.IsNullOrEmpty(txtCoverage.Text)) ||
                (cmbFoundry.SelectedIndex != -1 && (!String.IsNullOrEmpty(txtFPNo.Text) && !String.IsNullOrEmpty(txtCoverage.Text))));
        }
    }
}