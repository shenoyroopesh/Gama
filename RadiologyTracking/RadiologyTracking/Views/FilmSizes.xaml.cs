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
    public partial class FilmSizes : UserControl
    {
        public FilmSizes()
        {
            InitializeComponent();
        }

        private void filmDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ((DomainDataSourceView)filmSizesDataGrid.ItemsSource).Add(new FilmSize());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (filmSizesDataGrid.IsValid)
                //commit any unsaved changes to avoid an exception
                if (filmSizesDataGrid.CommitEdit())
                    filmDomainDataSource.DomainContext.SubmitChanges(Common.OnFormSubmitCompleted, null);
        }

        private void grdDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if (filmSizesDataGrid.CommitEdit())
                ((DomainDataSourceView)filmSizesDataGrid.ItemsSource).Remove((Energy)row.DataContext);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (filmSizesDataGrid.CommitEdit())
                filmDomainDataSource.DomainContext.RejectChanges();
        }
    }
}
