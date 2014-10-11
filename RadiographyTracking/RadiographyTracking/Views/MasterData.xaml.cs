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
    public partial class MasterData : BaseCRUDView
    {
        public MasterData()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAcceptanceAsPerAdd.Click += AddOperation;
            btnProcedureRefAdd.Click += AddOperation;
            btnSpecificationAdd.Click += AddOperation;
            btnObservationsAdd.Click += AddOperation;
            btnRetakeReasonsAdd.Click += AddOperation;

            btnAcceptanceAsPerCancel.Click += CancelOperation;
            btnProcedureRefCancel.Click += CancelOperation;
            btnSpecificationCancel.Click += CancelOperation;
            btnObservationsCancel.Click += CancelOperation;
            btnRetakeReasonsCancel.Click += CancelOperation;

            btnAcceptanceAsPerSave.Click += SaveOperation;
            btnProcedureRefSave.Click += SaveOperation;
            btnSpecificationSave.Click += SaveOperation;
            btnObservationsSave.Click += SaveOperation;
            btnRetakeReasonsSave.Click += SaveOperation;
        }

        private int typeOfGrid = 1;

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get
            {
                if (typeOfGrid == 1)
                    return this.acceptanceAsPerDataGrid;
                else if (typeOfGrid == 2)
                    return this.procedureRefDataGrid;
                else if (typeOfGrid == 3)
                    return this.specificationDataGrid;
                else if (typeOfGrid == 4)
                    return this.observationsDataGrid;
                else
                    return this.retakeReasonsDataGrid;

            }
        }

        public override DomainDataSource DomainSource
        {
            get
            {
                if (typeOfGrid == 1)
                    return this.acceptanceAsPerDomainDataSource;
                else if (typeOfGrid == 2)
                    return this.procedureRefDomainDataSource;
                else if (typeOfGrid == 3)
                    return this.specificationDomainDataSource;
                else if (typeOfGrid == 4)
                    return this.observationsDomainDataSource;
                else
                    return this.retakeReasonDomainDataSource;
            }
        }

        public override Type MainType
        {
            get
            {
                if (typeOfGrid == 1)
                    return typeof(AcceptanceAsPer);
                else if (typeOfGrid == 2)
                    return typeof(ProcedureReference);
                else if (typeOfGrid == 3)
                    return typeof(Specification);
                else if (typeOfGrid == 4)
                    return typeof(Observation);
                else
                    return typeof(RetakeReason);
            }
        }

        public override String ChangeContext
        {
            get
            {
                if (typeOfGrid == 1)
                    return "AcceptanceAsPer";
                else if (typeOfGrid == 2)
                    return "ProcedureReference";
                else if (typeOfGrid == 3)
                    return "Specification";
                else if (typeOfGrid == 4)
                    return "Observation";
                else
                    return "RetakeReason";
            }
        }

        public override String ChangeContextProperty
        {
            get
            {
                return "Value";
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (masterTab != null)
            {
                if (masterTab.SelectedIndex == 0)
                    typeOfGrid = 1;
                else if (masterTab.SelectedIndex == 1)
                    typeOfGrid = 2;
                else if (masterTab.SelectedIndex == 2)
                    typeOfGrid = 3;
                else if (masterTab.SelectedIndex == 3)
                    typeOfGrid = 4;
                else
                    typeOfGrid = 5;
                DomainSource.LoadedData += domainDataSource_LoadedData;
            }
        }

    }
}
