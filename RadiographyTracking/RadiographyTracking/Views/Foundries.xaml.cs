using System;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Models;
using Vagsons.Controls;

namespace RadiographyTracking.Views
{
    public partial class Foundries : BaseCRUDView
    {
        public Foundries()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;
            btnCancel.Click += CancelOperation;
            btnSave.Click += SaveOperation;

            this.ExcludePropertiesFromTracking.Add("Customers");
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.foundryDataGrid; }
        }

        public override DomainDataSource DomainSource
        { 
            get { return this.foundryDomainDataSource; } 
        }

        public override Type MainType
        {
            get { return typeof(Foundry); }
        }

        public override String ChangeContext
        {
            get
            {
                return "Foundry";
            }
        }

        public override String ChangeContextProperty
        {
            get
            {
                return "FoundryName";
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }
    }
}
