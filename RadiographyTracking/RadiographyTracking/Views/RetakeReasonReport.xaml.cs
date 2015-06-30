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
    public partial class RetakeReasonReport : BaseCRUDView
    {
        DataTable reportTable;
        RadiographyContext ctx;

        public RetakeReasonReport()
            : base()
        {
            InitializeComponent();
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiographyContext();
            busyIndicator.IsBusy = true;

            int foundryId = cmbFoundry.SelectedIndex == -1 ? -1 : ((Foundry)cmbFoundry.SelectedItem).ID;
            int retakeReasonId = cmbRetakeReason.SelectedIndex == -1 ? -1 : ((RetakeReason)cmbRetakeReason.SelectedItem).ID;
            ctx.Load(ctx.GetRetakeReasonReportsQuery(foundryId, (DateTime?)fromDatePicker.SelectedDate,
                (DateTime?)toDatePicker.SelectedDate, retakeReasonId)).Completed += loadCompleted;
        }

        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;
            var report = ((LoadOperation<RetakeReasonReportRow>)sender).Entities;
            retakeReasonReportGrid.ItemsSource = report;
            busyIndicator.IsBusy = false;

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            retakeReasonReportGrid.Export("Roopesh", "Gama", "", 0);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFetch.IsEnabled = ValueChanged();
        }

        public bool ValueChanged()
        {
            return (!(cmbFoundry.SelectedIndex == -1 || String.IsNullOrEmpty(fromDatePicker.Text) ||
                      String.IsNullOrEmpty(toDatePicker.Text))
                      || !(cmbRetakeReason.SelectedIndex == -1));
        }

        private void CmbFoundry_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fromDatePicker.Text = null;
            toDatePicker.Text = null;
            cmbRetakeReason.SelectedIndex = -1;
        }
    }
}
