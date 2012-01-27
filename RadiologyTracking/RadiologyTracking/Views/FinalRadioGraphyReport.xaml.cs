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
using System.Windows.Navigation;
using RadiologyTracking.Web.Models;
using Vagsons.Controls;
using System.Windows.Data;
using RadiologyTracking.Web.Services;
using System.Collections;
using System.ServiceModel.DomainServices.Client;

namespace RadiologyTracking.Views
{
    public partial class FinalRadioGraphyReport : BaseCRUDView
    {
        public FinalRadioGraphyReport()
            : base()
        {
            InitializeComponent();

            if (App.FinalReport == null)
            {
                //Means we came here by mistake go to casting status page                
                Navigate("/CastingStatusReport");
            }
            else
            {
                this.FinalReport = App.FinalReport;
                //DataContext = this.FinalReport;

                //set the query parameter here, the binding in the xaml won't work fine
                this.DomainSource.QueryParameters[0].Value = this.FinalReport.RTNo;
            }

            //wire up event handlers            
            AddEventHandlers();
            SetBindings();
            DomainSource.Load();            
        }

        /// <summary>
        /// Adds the required event handlers for all the page elements
        /// </summary>
        private void AddEventHandlers()
        {
            DomainSource.LoadedData += domainDataSource_LoadedData;
        }

        /// <summary>
        /// Sets the bindings of some properties to some of the UI elements, since they can't be bound directly in XAML. 
        /// </summary>
        private void SetBindings()
        {
            BindToPage(lblFixedPatternID, TextBlock.TextProperty, "FinalReport.FixedPatternID", BindingMode.OneWay);
            BindToPage(lblStatus, TextBlock.TextProperty, "FinalReport.Status.Status", BindingMode.OneWay);
            BindToPage(lblFPNo, TextBlock.TextProperty, "FinalReport.FixedPattern.FPNo", BindingMode.OneWay);
            BindToPage(lblCustomer, TextBlock.TextProperty, "FinalReport.FixedPattern.Customer.CustomerName", BindingMode.OneWay);
            BindToPage(lblDescription, TextBlock.TextProperty, "FinalReport.FixedPattern.Description", BindingMode.OneWay);
            BindToPage(lblCoverage, TextBlock.TextProperty, "FinalReport.Coverage.CoverageName", BindingMode.OneWay);
            BindToPage(lblRTNo, TextBlock.TextProperty, "FinalReport.RTNo", BindingMode.OneWay);
            BindToPage(txtLeadScreen, TextBlock.TextProperty, "FinalReport.LeadScreen");
            BindToPage(txtSourceSize, TextBlock.TextProperty, "FinalReport.SourceSize");
            BindToPage(txtReportNo, TextBlock.TextProperty, "FinalReport.ReportNo");
            BindToPage(txtHeatNo, TextBlock.TextProperty, "FinalReport.HeatNo");
            BindToPage(txtProcedureRef, TextBlock.TextProperty, "FinalReport.ProcedureRef");
            BindToPage(txtSpecifications, TextBlock.TextProperty, "FinalReport.Specifications");
            BindToPage(txtFilm, TextBlock.TextProperty, "FinalReport.Film");
            BindToPage(ReportDatePicker, TextBlock.TextProperty, "FinalReport.ReportDate");
            BindToPage(TestDatePicker, TextBlock.TextProperty, "FinalReport.DateOfTest");
            BindToPage(cmbShift, TextBlock.TextProperty, "FinalReport.Shift.Value");
            BindToPage(txtEvaluation, TextBlock.TextProperty, "FinalReport.EvaluationAsPer");
            BindToPage(txtAcceptance, TextBlock.TextProperty, "FinalReport.AcceptanceAsPer");
            BindToPage(txtDrawingNo, TextBlock.TextProperty, "FinalReport.DrawingNo");
            BindToPage(ReportDataGrid, CustomGrid.ItemsSourceProperty, "FinalReportRows");
        }


        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.ReportDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.ReportSource; }
        }

        public override Type MainType
        {
            get { return typeof(FinalRTReportRow); }
        }

        private FinalRTReport _finalReport;


        /// <summary>
        /// Current Fixed Pattern Template
        /// </summary>
        public FinalRTReport FinalReport
        {
            get
            {
                return this._finalReport;
            }
            set
            {
                this._finalReport = value;
                OnPropertyChanged("FinalReport");
            }
        }

        //kept ienumerable, so that the loaded object from the datacontext can be directly assigned here
        private IEnumerable<FinalRTReportRow> _finalReportRows;

        public IEnumerable<FinalRTReportRow> FinalReportRows
        {
            get
            {
                return _finalReportRows;
            }
            set
            {
                _finalReportRows = value;
                OnPropertyChanged("FinalReportRows");
            }
        }


        /// <summary>
        /// Event handler after the data source is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            base.domainDataSource_LoadedData(sender, e);
            //first item returned is the latest RG Report for the combination of inputs
            if (((DomainDataSourceView)((DomainDataSource)sender).Data).IsEmpty)
            {
                MessageBox.Show("Wrong Inputs Or RT No Already Completed\n\nCheck and try again");
                return;
            }

            FinalReport = (FinalRTReport)((DomainDataSourceView)((DomainDataSource)sender).Data).GetItemAt(0);
            FinalReportRows = FinalReport.FinalRTReportRows;
            DataContext = FinalReport;
            //now that fixedpatternid is available
            FixedPatternsSource.Load();
        }

        private void FixedPatternsSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            OnPropertyChanged("FinalReportRows");
        }

        private void chkOnlyRepairs_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)chkOnlyRepairs.IsChecked)
                FinalReportRows = FinalReport.FinalRTReportRows.Where(p => p.RemarkText != "ACCEPTABLE");
            else
                FinalReportRows = FinalReport.FinalRTReportRows;
        }
    }
}