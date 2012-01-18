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

namespace RadiologyTracking.Views
{
    public partial class EnterRadioGraphyReport : BaseCRUDView
    {
        public EnterRadioGraphyReport()
        {
            InitializeComponent();
            if (App.RGReport == null)
            {
                //Means new RG Report, allow add
            }
            else
            {
                this.RGReport = App.RGReport;
                DataContext = this.RGReport;
            }

            //wire up event handlers
            AddEventHandlers();
            SetBindings();
        }

        /// <summary>
        /// Adds the required event handlers for all the page elements
        /// </summary>
        private void AddEventHandlers()
        {
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;
            btnCancel.Click += CancelOperation;
            btnSave.Click += SaveOperation;
            FPTemplateRowsSource.LoadedData += FPTemplateRowsSource_LoadedData;
        }

        /// <summary>
        /// Sets the bindings of some properties to some of the UI elements, since they can't be bound directly in XAML. 
        /// </summary>
        private void SetBindings()
        {
            lblFPNo.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.FixedPattern.FPNo") });
            lblCustomer.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.FixedPattern.Customer.Name") });
            lblDescription.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.FixedPattern.Description") });
            lblCoverage.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.Coverage.Name") });
            lblRTNo.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.RTNo") });
            txtReportNo.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.ReportNo"), Mode = BindingMode.TwoWay });
            txtHeatNo.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.HeatNo"), Mode = BindingMode.TwoWay });
            txtProcedureRef.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.ProcedureRef"), Mode = BindingMode.TwoWay });
            txtSpecifications.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.Specifications"), Mode = BindingMode.TwoWay });
            txtFilm.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.Film"), Mode = BindingMode.TwoWay });
            ReportDatePicker.SetBinding(DatePicker.SelectedDateProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.ReportDate"), Mode = BindingMode.TwoWay });
            TestDatePicker.SetBinding(DatePicker.SelectedDateProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.DateOfTest"), Mode = BindingMode.TwoWay });
            cmbShift.SetBinding(ComboBox.SelectedValueProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.Shift"), Mode = BindingMode.TwoWay });
            txtEvaluation.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.EvaluationAsPer"), Mode = BindingMode.TwoWay });
            txtAcceptance.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.AcceptanceAsPer"), Mode = BindingMode.TwoWay });
            txtDrawingNo.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.DrawingNo"), Mode = BindingMode.TwoWay });
            RGReportDataGrid.SetBinding(CustomGrid.ItemsSourceProperty, new Binding() { Source = this, Path = new PropertyPath("RGReportRows") });
            btnAdd.SetBinding(Button.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("Enabled") });
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.RGReportDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.RGReportSource; }
        }

        public override Type MainType
        {
            get { return typeof(RGReportRow); }
        }

        public bool FetchEnabled
        {
            get { return (RGReport == null); }
        }

        public bool Enabled
        {
            get { return !(RGReport == null); }
        }

        private RGReport _rgReport;

        /// <summary>
        /// Current Fixed Pattern Template
        /// </summary>
        public RGReport RGReport
        {
            get
            {
                return this._rgReport;
            }
            set
            {
                this._rgReport = value;
                OnPropertyChanged("RGReport");
                OnPropertyChanged("Enabled");
            }
        }

        //kept ienumerable, so that the loaded object from the datacontext can be directly assigned here
        private IEnumerable _rgReportRows;

        public IEnumerable RGReportRows
        {
            get
            {
                return _rgReportRows;
            }
            set
            {
                _rgReportRows = value;
                OnPropertyChanged("RGReportRows");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DomainSource.Load();
        }

        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            //also give a few default empty string values so that UI copy operation is possible
            RGReportRow RGReportRow = new RGReportRow()
                                            {
                                                RGReport = this.RGReport,
                                                //auto increment sl no for each additional row
                                                SlNo = ((DomainDataSourceView)RGReportRows).Count + 1,
                                                Density = " ",
                                                Designation = " ",
                                                Location = " ",
                                                Segment = " ",
                                                Sensitivity = " ",
                                                FilmSizeString = " "
                                            };

            ((DomainDataSourceView)RGReportRows).Add(RGReportRow);
            OnPropertyChanged("RGReportRows");
        }

        public override void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            base.domainDataSource_LoadedData(sender, e);
            //first item returned is the current fixed pattern template for the given combination of fixed pattern and coverage
            RGReport = (RGReport)((DomainDataSourceView)((DomainDataSource)sender).Data).GetItemAt(0);
            //FPTemplateRowsSource.Load();
        }

        void FPTemplateRowsSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            RGReportRows = (DomainDataSourceView)((DomainDataSource)sender).Data;
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }

        private void FetchOperation(object sender, RoutedEventArgs e)
        {
            DomainSource.Load();
        }
    }
}