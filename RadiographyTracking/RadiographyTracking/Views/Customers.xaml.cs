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
using RadiographyTracking.Controls;

namespace RadiographyTracking.Views
{
    public partial class Customers : BaseCRUDView
    {
        public Customers()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;
            btnCancel.Click += CancelOperation;
            btnSave.Click += SaveOperation;
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.customersDataGrid; }
        }

        public override DomainDataSource DomainSource
        { 
            get { return this.customersDomainDataSource; } 
        }

        public override Type MainType
        {
            get { return typeof(Customer); }
        }

        public override String ChangeContext
        {
            get
            {
                return "Customer";
            }
        }

        public override String ChangeContextProperty
        {
            get
            {
                return "CustomerName";
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }

        private void FileUploadAdded(object sender, EventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            (row.DataContext as Customer).Logo = (sender as FileUpload).File;
        }
    }
}
