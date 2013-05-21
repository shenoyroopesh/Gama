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

namespace RadiographyTracking.Views
{
    public partial class Technicians : BaseCRUDView
    {
        public Technicians()
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
            get { return this.techniciansDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.techniciansDomainDataSource; }
        }

        public override Type MainType
        {
            get { return typeof(Technician); }
        }

        public override String ChangeContext
        {
            get
            {
                return "Technician";
            }
        }

        public override String ChangeContextProperty
        {
            get
            {
                return "Name";
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }
    }
}
