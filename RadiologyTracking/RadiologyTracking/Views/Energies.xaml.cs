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
using RadiologyTracking.Web.Models;

namespace RadiologyTracking.Views
{
    public partial class Energies : UserControl
    {
        public Energies()
        {
            InitializeComponent();
        }

        private void energyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ((DomainDataSourceView)energyDataGrid.ItemsSource).Add(new Energy());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            energyDomainDataSource.DomainContext.SubmitChanges();
        }
    }
}
