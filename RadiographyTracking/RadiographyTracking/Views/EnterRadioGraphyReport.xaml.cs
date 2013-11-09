﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Models;
using Vagsons.Controls;
using System.Windows.Data;
using RadiographyTracking.Web.Services;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;
using RadiographyTracking.Controls;
using System.Collections.Generic;
using RadiographyTracking.AddressStickers;

namespace RadiographyTracking.Views
{
    public partial class EnterRadioGraphyReport : BaseCRUDView
    {
        public EnterRadioGraphyReport()
            : base()
        {
            InitializeComponent();
            this.ExcludePropertiesFromTracking.Add("RGReport");
            this.ExcludePropertiesFromTracking.Add("RGReportRows");
            this.ExcludePropertiesFromTracking.Add("CanDelete");
            this.ExcludePropertiesFromTracking.Add("RowsDeleted");
            this.ExcludePropertiesFromTracking.Add("ThicknessRangeUI");
            this.ExcludePropertiesFromTracking.Add("Thickness");
            this.ExcludePropertiesFromTracking.Add("FilmSizeWithCount");
            this.ExcludePropertiesFromTracking.Add("Viewing");

            this.OnCancelNavigation = "/RadiographyReports";

            if (App.RGReport == null)
            {
                //Means new RG Report, allow add
                this.IsEditMode = false;
                //clerk or not
                string currentRole = WebContext.Current.User.Roles.FirstOrDefault();
                if (currentRole.ToLower() == "clerk")
                {
                    clerkMode = true;
                    this.RGReportDataGrid.Visibility = Visibility.Collapsed;
                    this.RGReportDataGridClerk.Visibility = Visibility.Visible;

                    //make sure the rows are not frozen for the clerk-specific datagrid
                    CustomGrid.IsEditAllowed = true;
                }
            }
            else
            {
                this.RGReport = App.RGReport;
                DataContext = this.RGReport;
                this.IsEditMode = true;
                //set the query parameter here, the binding in the xaml is not working fine
                this.EditRGReportsSource.QueryParameters[0].Value = this.RGReport.ID;
            }

            //wire up event handlers            
            AddEventHandlers();
            SetBindings();

            if (IsEditMode) DomainSource.Load();

            

            //var ctx = new RadiographyContext();
            //ctx.Load(ctx.GetEndCustomerNames());
        }

        private bool clerkMode = false;

        /// <summary>
        /// ChangeContext
        /// </summary>
        public override String ChangeContext
        {
            get { return "RGReport"; }
        }

        public override String ChangeContextValue
        {
            get { return RGReport != null ? String.Concat(RGReport.RTNo, " Row") : String.Empty; }
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
            BindToPage(txtTotalArea, TextBlock.TextProperty, "TotalArea", BindingMode.OneWay);
            BindToPage(lblFixedPatternID, TextBlock.TextProperty, "RGReport.FixedPatternID", BindingMode.OneWay);
            BindToPage(lblRGReportID, TextBlock.TextProperty, "RGReport.ID", BindingMode.OneWay);
            BindToPage(lblStatus, TextBlock.TextProperty, "RGReport.Status.Status", BindingMode.OneWay);
            BindToPage(lblFPNo, TextBlock.TextProperty, "RGReport.FixedPattern.FPNo", BindingMode.OneWay);
            BindToPage(lblCustomer, TextBlock.TextProperty, "RGReport.FixedPattern.Customer.CustomerName", BindingMode.OneWay);
            BindToPage(lblDescription, TextBlock.TextProperty, "RGReport.FixedPattern.Description", BindingMode.OneWay);
            BindToPage(lblCoverage, TextBlock.TextProperty, "RGReport.Coverage.CoverageName", BindingMode.OneWay);
            BindToPage(lblRTNo, TextBlock.TextProperty, "RGReport.RTNo", BindingMode.OneWay);
            BindToPage(txtLeadScreen, TextBox.TextProperty, "RGReport.LeadScreen");
            BindToPage(txtSource, TextBox.TextProperty, "RGReport.Source");
            BindToPage(txtStrength, TextBox.TextProperty, "RGReport.Strength");
            BindToPage(txtSourceSize, TextBox.TextProperty, "RGReport.SourceSize");
            BindToPage(txtReportNo, TextBox.TextProperty, "RGReport.ReportNo");
            BindToPage(txtHeatNo, TextBox.TextProperty, "RGReport.HeatNo");
            BindToPage(txtProcedureRef, TextBox.TextProperty, "RGReport.ProcedureRef");
            BindToPage(txtSpecifications, TextBox.TextProperty, "RGReport.Specifications");
            BindToPage(txtEndCustomerName, AutoCompleteBox.TextProperty, "RGReport.EndCustomerName");
            BindToPage(txtFilm, TextBox.TextProperty, "RGReport.Film");
            BindToPage(ReportDatePicker, DatePicker.SelectedDateProperty, "RGReport.ReportDate");
            BindToPage(TestDatePicker, DatePicker.SelectedDateProperty, "RGReport.DateOfTest");
            BindToPage(cmbShift, ComboBox.SelectedValueProperty, "RGReport.Shift");
            BindToPage(txtEvaluation, TextBlock.TextProperty, "RGReport.EvaluationAsPer");
            BindToPage(txtAcceptance, TextBox.TextProperty, "RGReport.AcceptanceAsPer");
            BindToPage(lblViewing, TextBlock.TextProperty, "RGReport.Viewing");
            BindToPage(txtDrawingNo, TextBox.TextProperty, "RGReport.DrawingNo");
            BindToPage(RGReportDataGrid, CustomGrid.ItemsSourceProperty, "RGReportRows");
            BindToPage(RGReportDataGridClerk, CustomGrid.ItemsSourceProperty, "RGReportRows");
            BindToPage(btnAdd, Button.IsEnabledProperty, "Enabled", BindingMode.OneWay);
            BindToPage(btnFetch, Button.IsEnabledProperty, "FetchEnabled", BindingMode.OneWay);
            BindToPage(busyIndicator, BusyIndicator.IsBusyProperty, "DomainSource.IsBusy", BindingMode.OneWay);
        }


        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return clerkMode ? RGReportDataGridClerk : RGReportDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return IsEditMode ? EditRGReportsSource : RGReportSource; }
        }

        public override Type MainType
        {
            get { return typeof(RGReportRow); }
        }

        public bool IsEditMode { get; set; }

        public bool FetchEnabled
        {
            get { return !IsEditMode; }
        }

        public bool Enabled
        {
            get { return RGReport != null; }
        }

        private RGReport _rgReport;

        /// <summary>
        /// Current Fixed Pattern Template
        /// </summary>
        public RGReport RGReport
        {
            get { return _rgReport; }
            set
            {
                _rgReport = value;
                OnPropertyChanged("RGReport");
                OnPropertyChanged("Enabled");
                OnPropertyChanged("FetchEnabled");
            }
        }

        //kept ienumerable, so that the loaded object from the datacontext can be directly assigned here
        private EntityCollection<RGReportRow> _rgReportRows;

        public EntityCollection<RGReportRow> RGReportRows
        {
            get { return _rgReportRows; }
            set
            {
                _rgReportRows = value;
                OnPropertyChanged("RGReportRows");

            }
        }

 
        /// <summary>
        /// Total area for the entire report
        /// </summary>
        public String TotalArea
        {
            get
            {
                return (RGReportRows == null
                            ? 0
                            : RGReportRows
                                  .Where(p => p.RemarkText != "RETAKE")
                                  .Sum(p => (p.FilmSize == null ? 0 : p.FilmSize.Area * p.FilmCount)))
                           .ToString() + " Sq. Inches";
            }
        }

   

        public void UpdateEnergyWiseArea()
        {
            var ctx = (RadiographyContext)this.DomainSource.DomainContext;
            var dt = new DataTable("EnergyTable");
            AddTextColumn(dt, "HeadRow", "HeadRow");
            var headerRow = new DataRow();
            var actualRow = new DataRow();
            headerRow["HeadRow"] = "Isotope";
            actualRow["HeadRow"] = "Sq. Inches";

            //instead of encountering an error if context is still loading, just don't do it, it will get 
            //done on the first save operation
            if (ctx.IsLoading)
                return;

            foreach (var e in ctx.Energies)
            {
                AddTextColumn(dt, e.Name, e.Name);
                headerRow[e.Name] = e.Name;
                actualRow[e.Name] = RGReportRows
                    .Where(p => p.EnergyID == e.ID &&
                                p.RemarkText != "RETAKE")
                    //30-Jun-12 - Roopesh added this to ensure that retake areas are not included
                    .Sum(p => p.FilmSize.Area * p.FilmCount);
            }

            dt.Rows.Add(headerRow);
            dt.Rows.Add(actualRow);

            energyAreas.DataSource = dt;
            energyAreas.DataBind();

            OnPropertyChanged("TotalArea");
      
           }

        private static void AddTextColumn(DataTable reportTable, String columnName, String caption)
        {
            var dc = new DataColumn(columnName)
                {
                    Caption = caption,
                    ReadOnly = true,
                    DataType = typeof(String),
                    AllowResize = true,
                    AllowSort = false,
                    AllowReorder = false
                };
            reportTable.Columns.Add(dc);
        }


        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            //also give a few default empty string values so that UI copy operation is possible
            var rgReportRow = new RGReportRow
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
                    WelderText = " ",
                    FilmCount = 1, //default value for film counts
                    RowType = ((RadiographyContext)DomainSource.DomainContext)
                        .RGReportRowTypes
                        .FirstOrDefault(p => p.Value == "FRESH")
                };

            RGReportRows.Add(rgReportRow);
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
          
            //now that fixedpatternid is available
            FixedPatternsSource.Load();
            UpdateEnergyWiseArea();
            OnPropertyChanged("TotalArea");
            SetViewing();

            //if edit mode, add a clone of original RGReport to original entities for change tracking
            if (IsEditMode)
            {
                OriginalEntities.Add(RGReport.ID, RGReport.Clone(ExcludePropertiesFromTracking));
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            if (
                MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete",
                                MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if (Grid.CommitEdit())
            {
                RGReportRows.Remove((RGReportRow)row.DataContext);
                //also delete from the datacontext
                ((RadiographyContext)this.DomainSource.DomainContext).RGReportRows
                                                                      .Remove(row.DataContext as RGReportRow);

                //mark at least one row deleted
                RGReport.RowsDeleted = true;
            }
        }

        private void FetchOperation(object sender, RoutedEventArgs e)
        {
            //ensure that the mandatory fields are filled
            if (String.IsNullOrEmpty(this.txtFPNo.Text) ||
                String.IsNullOrEmpty(this.txtRTNo.Text) ||
                this.cmbCoverage.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill FP No, Coverage and RT No before fetching");
                return;
            }

            this.IsEditMode = false;
            this.RGReportDataGrid.CommitEdit();
            if (DomainSource.HasChanges) DomainSource.RejectChanges();
            DomainSource.Load();
        }

        public override void SaveOperation(object sender, RoutedEventArgs e)
        {
            

            //validations on each row
            foreach (var row in RGReportRows)
            {
                //sl no should be unique
                var duplicateRow = RGReportRows.FirstOrDefault(p => p.SlNo == row.SlNo && p != row);
                if (duplicateRow != null)
                {
                    MessageBox.Show(string.Format("Sl no {0} has been repeated twice, Correct this before saving",
                                                  row.SlNo));
                    return;
                }

                //Location + segment uniqueness validation validation
                var conflictingRow =
                    RGReportRows.FirstOrDefault(
                        p => p.SlNo != row.SlNo && p.Location == row.Location && p.Segment == row.Segment);
                if (conflictingRow != null)
                {
                    MessageBox.Show(
                        string.Format(
                            "Rows with Sl No {0} and {1} have the same location and segments. Correct this before saving",
                            row.SlNo, conflictingRow.SlNo));
                    return;
                }
            }

            //set the viewing
            SetViewing();

      /** FOR SIMPLICITY OF DESIGN, SERVER DEPENDS ON THE CLIENT TO SET THE STATUS. THIS IS IMPORTANT! WITHOUT THIS THE LOGIC WILL FAIL **/

            MessageBoxResult result;
            if (RGReportRows.Any(p => p.RemarkText.Trim() == String.Empty))
            {
                result = MessageBox.Show("Save Incomplete Report. Fetching this RT No will fetch Same Report again",
                                         "Confirm Save", MessageBoxButton.OKCancel);
            }
            //for the first report, deleted rows do not affect status of the casting but for later reports if even a single row
            // is deleted, then the report can never be the final report (hence the casting will remain in pending state)
            else if (RGReportRows.Any(p => p.RemarkText.ToUpper() != "ACCEPTABLE") ||
                     ((!RGReport.First) && RGReport.RowsDeleted))
            {
                result =
                    MessageBox.Show(
                        "Mark Casting as Pending. At least one report is needed after this for this RT No",
                        "Confirm Save", MessageBoxButton.OKCancel);

                if (result != MessageBoxResult.Cancel)
                    RGReport.Status =
                        ((RadiographyContext)DomainSource.DomainContext).RGStatus.FirstOrDefault(
                            p => p.Status == "PENDING");
            }
            else
            {
                result = MessageBox.Show("Mark Casting as complete. This will be Last Report for this RT No.",
                                         "Confirm Save", MessageBoxButton.OKCancel);

                if (result != MessageBoxResult.Cancel)
                    RGReport.Status =
                        ((RadiographyContext)DomainSource.DomainContext).RGStatus.FirstOrDefault(
                            p => p.Status == "COMPLETE");
            }

            //allow cancel
            if (result == MessageBoxResult.Cancel)
                return;

            base.SaveOperation(sender, e);

            //update the energy grid
            UpdateEnergyWiseArea();

            //clear and repopulate the original entities collection
            OriginalEntities.Clear();
            OriginalEntities.Add(RGReport.ID, RGReport.Clone(ExcludePropertiesFromTracking));
        }

        private void SetViewing()
        {
            RGReport.Viewing = (RGReport.RGReportRows.Any(p => p.FilmCount > 1)) ? "Single And Double" : "Single";
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

        private void TechniqueChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            ((RGReportRow)row).Technique = cmb.SelectedValue.ToString();
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
            if (cmb.SelectedValue == null) return;
            ((RGReportRow)row).FilmSizeString = ((FilmSize)cmb.SelectedValue).Name;
        }


        /// <summary>
        /// Have to update filmsize string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnergyChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            if (cmb.SelectedValue == null) return;
            ((RGReportRow)row).EnergyText = ((Energy)cmb.SelectedValue).Name;
        }

        #endregion

        private void FilmSizeAreaLoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //in case it din't load earlier
            UpdateEnergyWiseArea();
        }


        public override void OnFormSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message), "Error", MessageBoxButton.OK);
                so.MarkErrorAsHandled();
            }
            else
            {
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButton.OK);
                var addressStickersWindow = new PrintAddressStickers
                    {
                        ReportNo = this.RGReport.ReportNo
                    };
                addressStickersWindow.Show();
            }
        }

        private void txtEndCustomer_Populating(object sender, PopulatingEventArgs e)
        {
            var ctx = new RadiographyContext();
            if (RGReport != null)
                ctx.GetEndCustomerNames(EndCustomerNames_Loaded, null);
        }

        private void EndCustomerNames_Loaded(InvokeOperation<IEnumerable<string>> op)
        {
            txtEndCustomerName.ItemsSource = op.Value;
            txtEndCustomerName.PopulateComplete();
        }


    }
}