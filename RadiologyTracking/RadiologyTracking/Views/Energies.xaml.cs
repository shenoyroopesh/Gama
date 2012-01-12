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
using System.ServiceModel.DomainServices.Client;
using System.ComponentModel.DataAnnotations;

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
           if(energyDataGrid.IsValid)
                //commit any unsaved changes to avoid an exception
                if(energyDataGrid.CommitEdit())
                    energyDomainDataSource.DomainContext.SubmitChanges(Common.OnFormSubmitCompleted, null);
        }

        private void grdDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if(energyDataGrid.CommitEdit()) 
                ((DomainDataSourceView)energyDataGrid.ItemsSource).Remove((Energy)row.DataContext);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(energyDataGrid.CommitEdit())
                energyDomainDataSource.DomainContext.RejectChanges();
        }
    }
}
