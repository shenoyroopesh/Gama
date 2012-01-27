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
using RadiologyTracking.Web.Services;
using System.Threading;
using RadiologyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;
using System.Collections;
using RadiologyTracking.Controls;
using System.Windows.Data;


namespace RadiologyTracking.Views
{
    public partial class FilmStockReport : BaseCRUDView
    {
        public FilmStockReport()
            : base()
        {
            InitializeComponent();
            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            reportSource.Load();
        }

        private void loadCompleted(object sender, EventArgs e)
        {
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            reportGrid.Export("Roopesh", "Gama", "", 0);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFetch.IsEnabled = !(cmbFoundry.SelectedIndex == -1 || String.IsNullOrEmpty(fromDatePicker.Text) || 
                                  String.IsNullOrEmpty(toDatePicker.Text));
        }
    }
}
