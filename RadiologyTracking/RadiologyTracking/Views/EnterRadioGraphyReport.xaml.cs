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
        }

        /// <summary>
        /// Sets the bindings of some properties to some of the UI elements, since they can't be bound directly in XAML. 
        /// </summary>
        private void SetBindings()
        {
            lblFPNo.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.FixedPattern.FPNo") });
            lblCustomer.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.FixedPattern.Customer.CustomerName") });
            lblDescription.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.FixedPattern.Description") });
            lblCoverage.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.Coverage.CoverageName") });
            lblRTNo.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.RTNo") });
            txtLeadScreen.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.LeadScreen"), Mode = BindingMode.TwoWay });
            txtSourceSize.SetBinding(TextBox.TextProperty, new Binding() { Source = this, Path = new PropertyPath("RGReport.SourceSize"), Mode = BindingMode.TwoWay });
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
            btnFetch.SetBinding(Button.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("FetchEnabled") });
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
        private EntityCollection<RGReportRow> _rgReportRows;

        public EntityCollection<RGReportRow> RGReportRows
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

        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            //also give a few default empty string values so that UI copy operation is possible
            RGReportRow RGReportRow = new RGReportRow()
                                            {
                                                RGReport = this.RGReport,
                                                //auto increment sl no for each additional row
                                                SlNo = RGReportRows.Max(p => p.SlNo) + 1,
                                                Density = " ",
                                                Designation = " ",
                                                Location = " ",
                                                Segment = " ",
                                                Sensitivity = " ",
                                                FilmSizeString = " ",
                                                RemarkText = " ",
                                                TechnicianText = " ",
                                                WelderText = " "
                                            };

            RGReportRows.Add(RGReportRow);
            OnPropertyChanged("RGReportRows");
        }

        public override void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            base.domainDataSource_LoadedData(sender, e);
            //first item returned is the latest RG Report for the combination of inputs
            if (((DomainDataSourceView)((DomainDataSource)sender).Data).IsEmpty)
            {
                MessageBox.Show("Wrong Inputs Or RT No Already Completed\n\nCheck and try again");
                return;
            }

            RGReport = (RGReport)((DomainDataSourceView)((DomainDataSource)sender).Data).GetItemAt(0);
            RGReportRows = RGReport.RGReportRows;
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if (Grid.CommitEdit())
                RGReportRows.Remove((RGReportRow)row.DataContext);
        }

        private void FetchOperation(object sender, RoutedEventArgs e)
        {
            DomainSource.Load();
            //for fixed pattern related data
            FixedPatternsSource.Load();
        }


        public override void SaveOperation(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            
            /** FOR SIMPLICITY OF DESIGN, SERVER DEPENDS ON THE CLIENT TO SET THE STATUS. THIS IS IMPORTANT! WITHOUT THIS THE LOGIC WILL FAIL **/

            if (RGReportRows.Where(p => p.RemarkText.Trim() == String.Empty).Count() > 0)
            {
                result = MessageBox.Show("Save Incomplete Report. Fetching this RT No will fetch Same Report again", 
                    "Confirm Save", MessageBoxButton.OKCancel);                
            }
            else if (RGReportRows.Where(p => p.RemarkText.ToUpper() != "ACCEPTABLE").Count() > 0)
            {
                result = MessageBox.Show("Mark Casting as Pending. At least one report is needed after this for this RT No", 
                    "Confirm Save", MessageBoxButton.OKCancel);

                if (result != MessageBoxResult.Cancel)
                    RGReport.Status = ((RadiologyContext)DomainSource.DomainContext).RGStatus.FirstOrDefault(p => p.Status == "PENDING");
            }
            else
            {
                result = MessageBox.Show("Mark Casting as complete. This will be Last Report for this RT No.", 
                    "Confirm Save", MessageBoxButton.OKCancel);

                if (result != MessageBoxResult.Cancel) 
                    RGReport.Status = ((RadiologyContext)DomainSource.DomainContext).RGStatus.FirstOrDefault(p => p.Status == "COMPLETE");
            }

            //allow cancel
            if (result == MessageBoxResult.Cancel)
                return;

            base.SaveOperation(sender, e);
        }

        #region combobox change handlers
        
        //TODO: Check if these four methods can be abstracted into one method

        /// <summary>
        /// Have to update Technician string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TechnicianChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            ((RGReportRow)row).TechnicianText = ((Technician)cmb.SelectedValue).Name;
        }

        /// <summary>
        /// Have to update Welder string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WelderChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            ((RGReportRow)row).WelderText = ((Welder)cmb.SelectedValue).Name;
        }

        /// <summary>
        /// Have to update Remark string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemarkChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            ((RGReportRow)row).RemarkText = ((Remark)cmb.SelectedValue).Value;
        }

        /// <summary>
        /// Have to update filmsize string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilmSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            ((FPTemplateRow)row).FilmSizeString = ((FilmSize)cmb.SelectedValue).Name;
        }

        #endregion
    }
}