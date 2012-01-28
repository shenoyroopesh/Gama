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
using Vagsons.Controls;

namespace RadiologyTracking.Views
{
    public partial class ChangesMadeReport : BaseCRUDView
    {
        public ChangesMadeReport()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.changesDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.changesDomainDataSource; } 
        }

        public override Type MainType
        {
            get { return typeof(Change); }
        }
    }
}
