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
using RadiologyTracking.Web.Models;

namespace RadiologyTracking.Views
{
    public partial class ThicknessToEnergyType : UserControl
    {
        public ThicknessToEnergyType()
        {
            InitializeComponent();
        }

        private void thicknessRangeForEnergyDomainDataSource_LoadedData(object sender, System.Windows.Controls.LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ((DomainDataSourceView)grdThicknessRangeForEnergy.ItemsSource).Add(new ThicknessRangeForEnergy());
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (grdThicknessRangeForEnergy.IsValid)
                //commit any unsaved changes to avoid an exception
                if (grdThicknessRangeForEnergy.CommitEdit())
                    thicknessRangeForEnergyDomainDataSource.DomainContext.SubmitChanges(Common.OnFormSubmitCompleted, null);
        }

        private void grdDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if (grdThicknessRangeForEnergy.CommitEdit())
                ((DomainDataSourceView)grdThicknessRangeForEnergy.ItemsSource).Remove((ThicknessRangeForEnergy)row.DataContext);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (grdThicknessRangeForEnergy.CommitEdit())
                thicknessRangeForEnergyDomainDataSource.DomainContext.RejectChanges();
        }

        private void energyDataSource_Loaded(object sender, RoutedEventArgs e)
        {
            var test = sender;
        }
    }
}
