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

namespace RadiographyTracking.Views
{
    public partial class FixedPatterns : BaseCRUDView
    {
        public FixedPatterns()
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
            get { return this.FPDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.FPDomainDataSource; }
        }

        public override Type MainType
        {
            get { return typeof(FixedPattern); }
        }

        public override String ChangeContext
        {
            get
            {
                return "Fixed Pattern";
            }
        }

        public override String ChangeContextProperty
        {
            get
            {
                return "FPNo";
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }

        private void grdTemplatesButton_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            App.FixedPattern = (FixedPattern)row.DataContext;
            Navigate("/FixedPatternTemplates");
        }
    }
}
