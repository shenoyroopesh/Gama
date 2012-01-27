﻿using System;
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
    }
}
