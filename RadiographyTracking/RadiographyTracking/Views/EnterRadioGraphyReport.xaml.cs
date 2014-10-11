using System;
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
using RadiographyTracking.Observations;

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
            this.ExcludePropertiesFromTracking.Add("FilmSizeStringInCms");
            this.ExcludePropertiesFromTracking.Add("FilmSizeWithCountInCms");
            this.ExcludePropertiesFromTracking.Add("TotalAreaInCms");


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
            UpdateEnergyWiseArea();
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
            BindToPage(lblFixedPatternID, TextBlock.TextProperty, "RGReport.FixedPatternID", BindingMode.OneWay);
            BindToPage(lblFPNo, TextBlock.TextProperty, "RGReport.FixedPattern.FPNo", BindingMode.OneWay);
            BindToPage(lblCustomer, TextBlock.TextProperty, "RGReport.FixedPattern.Customer.CustomerName", BindingMode.OneWay);
            BindToPage(lblDescription, TextBlock.TextProperty, "RGReport.FixedPattern.Description", BindingMode.OneWay);
            BindToPage(txtTotalArea, TextBlock.TextProperty, "TotalArea", BindingMode.OneWay);
            BindToPage(lblRGReportID, TextBlock.TextProperty, "RGReport.ID", BindingMode.OneWay);
            BindToPage(lblStatus, TextBlock.TextProperty, "RGReport.Status.Status", BindingMode.OneWay);
            BindToPage(lblCoverage, TextBlock.TextProperty, "RGReport.Coverage.CoverageName", BindingMode.OneWay);
            BindToPage(lblRTNo, TextBlock.TextProperty, "RGReport.RTNo", BindingMode.OneWay);
            BindToPage(txtLeadScreen, TextBox.TextProperty, "RGReport.LeadScreen");
            BindToPage(txtLeadScreenBack, TextBox.TextProperty, "RGReport.LeadScreenBack");
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
            if (ctx.Energies != null && RGReportRows != null)
            {
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
                    Observations = " ",
                    WelderText = " ",
                    RetakeReasonText = " ",
                    FilmCount = 1, //default value for film counts
                    RowType = ((RadiographyContext)DomainSource.DomainContext)
                        .RGReportRowTypes
                        .FirstOrDefault(p => p.Value == "FRESH")
                };

            RGReportRows.Add(rgReportRow);
            OnPropertyChanged("RGReportRows");
        }

        protected DateTime reportDateOriginalValue;
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
            UpdatedStatus();
            if (isFromFetchMethod)
            {
                var ctx = (RadiographyContext)this.DomainSource.DomainContext;
                Coverage coverage = (Coverage)cmbCoverage.SelectedItem;
                ctx.Load(ctx.GetFixedPatternDetailsQuery(txtFPNo.Text, coverage.CoverageName, txtRTNo.Text));

                UpdateSourceBasedOnThickness();
                RGReport.FixedPattern = ctx.FixedPatterns.FirstOrDefault();
            }
            isFromFetchMethod = false;
            //if edit mode, add a clone of original RGReport to original entities for change tracking
            if (IsEditMode)
            {
                OriginalEntities.Add(RGReport.ID, RGReport.Clone(ExcludePropertiesFromTracking));
            }
            //ProcedureReference();
            BindToPage(cmbProcedureRef, ComboBox.SelectedValueProperty, "ProcedureReferences");
            BindToPage(cmbSpecifications, ComboBox.SelectedValueProperty, "Specifications");
            BindToPage(cmbAcceptance, ComboBox.SelectedValueProperty, "AcceptanceAsPers");
        }

        public void UpdatedStatus()
        {
            if (RGReport.StatusID == 2)
            {
                if (RGReportRows.SelectMany(p => p.Classifications).Count() > 0)
                    lblStatus.Text = "CASTING ACCEPTABLE AS PER LEVEL " + RGReportRows.SelectMany(p => p.Classifications.Split(',')).Where(m => !string.IsNullOrEmpty(m)).Select(int.Parse).Max();
                else
                    lblStatus.Text = "CASTING ACCEPTABLE AS PER LEVEL 1";
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
                var rowToBeRemoved = (RGReportRow)row.DataContext;
                RGReportRows.Remove(rowToBeRemoved);
                //also delete from the datacontext
                ((RadiographyContext)this.DomainSource.DomainContext).RGReportRows
                                                                      .Remove(rowToBeRemoved as RGReportRow);

                //mark at least one row deleted
                RGReport.RowsDeleted = true;
            }
            UpdateSourceBasedOnThickness();
        }
        private bool isFromFetchMethod = false;

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
            UpdateEnergyWiseArea();
            UpdateSourceBasedOnThickness();
            isFromFetchMethod = true;
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
                            p => p.Status == "CASTING UNDER REPAIR");
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
            UpdateReportDate();
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
            if (cmb.Parent != null)
            {
                var row = ((DataGridCell)cmb.Parent).DataContext;
                ((RGReportRow)row).RemarkText = ((Remark)cmb.SelectedValue).Value;

                var ctx = (RadiographyContext)this.DomainSource.DomainContext;
                ctx.Load(ctx.GetRetakeReasonsQuery());

                if (((RGReportRow)row).RemarkText != "RETAKE")
                {
                    ((RGReportRow)row).RetakeReasonID = null;
                    ((RGReportRow)row).RetakeReasonText = string.Empty;
                }
                else
                {
                    if (((RGReportRow)row).RetakeReasonID == null)
                    {
                        RetakeReason retakeReason = ctx.RetakeReasons.FirstOrDefault();
                        ((RGReportRow)row).RetakeReasonText = retakeReason.Value;
                    }
                }
            }
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
                        ReportNo = this.RGReport.ReportNo,
                        RGReportID = Convert.ToString(this.RGReport.ID)
                    };
                addressStickersWindow.Show();
            }
            UpdatedStatus();
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

        private void CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
        {
            if (e.Column.Header.ToString() == "Energy")
            {
                UpdateSourceBasedOnThickness();
            }
        }

        public void UpdateSourceBasedOnThickness()
        {
            if (RGReport != null && RGReport.ReshootNo < 1 && RGReportRows != null)
            {
                if (RGReportRows.Select(o => o.EnergyID).Distinct().Count() == 1)
                {
                    RGReportRow RGReportRow = RGReportRows.FirstOrDefault();
                    if (RGReportRow.EnergyID == 1)
                    {
                        this.txtSource.Text = "Co 60";
                        this.txtSourceSize.Text = "4.1 mm(Effective size)";
                    }
                    else
                    {
                        this.txtSource.Text = "Ir 192";
                        this.txtSourceSize.Text = "3.6 mm(Effective size)";
                    }
                }
                else
                {
                    this.txtSource.Text = "Co 60/Ir 192";
                    this.txtSourceSize.Text = "4.1 mm(Effective size)/3.6 mm(Effective size)";
                }
            }
        }

        //Added by praveen to fix date issue that was causing.(Requirement:3rd july, 2014)
        protected void UpdateReportDate()
        {
            var ctx = (RadiographyContext)this.DomainSource.DomainContext;
            int reShootNo = RGReport.ReshootNo - 1;
            String rTNo = RGReport.RTNo;
            if (RGReport.ReshootNo > 0)
            {
                ctx.Load(ctx.GetRGReportsOnRtNoAndReshootNoQuery(rTNo, reShootNo));
                RGReport rGReport = ctx.RGReports.Where(p => p.ReshootNo == reShootNo &&
                                                    p.RTNo == rTNo).FirstOrDefault();

                if (RGReport != null && rGReport != null)
                {
                    if (rGReport.ReportDate.ToString("dd-MM-yyyy") == Convert.ToDateTime(ReportDatePicker.Text).ToString("dd-MM-yyyy"))
                    {
                        RGReport.ReportDate = rGReport.ReportDate.AddMinutes(1);
                    }
                }
            }
        }

        /// <summary>
        /// Have to update Retake Reason string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RetakeReasonChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb.Parent != null)
            {
                var row = ((DataGridCell)cmb.Parent).DataContext;
                if (cmb.Parent != null)
                {
                    if (cmb.SelectedIndex > -1)
                    {
                        ((RGReportRow)row).RetakeReasonText = ((RetakeReason)cmb.SelectedValue).Value;
                        if (((RGReportRow)row).RemarkText == "RETAKE")
                            cmb.IsEnabled = true;
                        else
                        {
                            cmb.IsEnabled = false;
                            ((RGReportRow)row).RetakeReasonText = string.Empty;
                            ((RGReportRow)row).RetakeReasonID = null;
                        }
                    }
                }
            }
        }

        private void PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            var row = e.Row.DataContext;
            ComboBox cmb = this.RGReportDataGrid.Columns[14].GetCellContent(e.Row) as ComboBox;
            if (cmb != null)
                if (((RGReportRow)row).RemarkText == "RETAKE")
                {
                    cmb.IsEnabled = true;
                    if (((RGReportRow)row).RetakeReasonID == null)
                    {
                        cmb.SelectedIndex = 0;
                    }
                }
                else
                {
                    cmb.IsEnabled = false;
                    cmb.SelectedIndex = -1;
                }
        }

        private void PreparingCellForEditClerkGrid(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            var row = e.Row.DataContext;
            ComboBox cmb = this.RGReportDataGridClerk.Columns[14].GetCellContent(e.Row) as ComboBox;
            if (cmb != null)
                if (((RGReportRow)row).RemarkText == "RETAKE")
                    cmb.IsEnabled = true;
                else
                {
                    cmb.IsEnabled = false;
                    cmb.SelectedIndex = -1;
                }
        }

        /// <summary>
        /// cmbProcedureRef combobox change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbProcedureRefChanged(object sender, SelectionChangedEventArgs e)
        {
            ProcedureReference procedureReference = (ProcedureReference)cmbProcedureRef.SelectedItem;
            txtProcedureRef.Text = procedureReference.Value;
        }

        /// <summary>
        /// cmbProcedureRef combobox change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSpecificationsChanged(object sender, SelectionChangedEventArgs e)
        {
            Specification specification = (Specification)cmbSpecifications.SelectedItem;
            txtSpecifications.Text = specification.Value;
        }

        /// <summary>
        /// cmbProcedureRef combobox change event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAcceptanceChanged(object sender, SelectionChangedEventArgs e)
        {
            AcceptanceAsPer acceptanceAsPer = (AcceptanceAsPer)cmbAcceptance.SelectedItem;
            txtAcceptance.Text = acceptanceAsPer.Value;
        }

        /// <summary>
        /// Property to set the Procedure textbox or combobox.
        /// </summary>
        public ProcedureReference ProcedureReferences
        {
            get
            {
                ProcedureReference procedureReference = new ProcedureReference();
                var ctx = (RadiographyContext)this.DomainSource.DomainContext;
                ctx.Load(ctx.GetProcedureRefsQuery());
                if (RGReport.ProcedureRef != null)
                {
                    if (ctx.ProcedureReferences.Where(p => p.Value == RGReport.ProcedureRef.Trim()).FirstOrDefault() == null)
                    {
                        cmbProcedureRef.Visibility = Visibility.Collapsed;
                        txtProcedureRef.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        cmbProcedureRef.Visibility = Visibility.Visible;
                        txtProcedureRef.Visibility = Visibility.Collapsed;
                    }
                    return ctx.ProcedureReferences.Where(p => p.Value == RGReport.ProcedureRef.Trim()).FirstOrDefault();
                }
                else
                    return procedureReference;
            }
        }

        /// <summary>
        /// Property to set the Specifications textbox or combobox.
        /// </summary>
        public Specification Specifications
        {
            get
            {
                Specification specification = new Specification();
                var ctx = (RadiographyContext)this.DomainSource.DomainContext;
                ctx.Load(ctx.GetSpecificationsQuery());
                if (RGReport.Specifications != null)
                {
                    if (ctx.Specifications.Where(p => p.Value == RGReport.Specifications.Trim()).FirstOrDefault() == null)
                    {
                        cmbSpecifications.Visibility = Visibility.Collapsed;
                        txtSpecifications.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        cmbSpecifications.Visibility = Visibility.Visible;
                        txtSpecifications.Visibility = Visibility.Collapsed;
                    }
                    return ctx.Specifications.Where(p => p.Value == RGReport.Specifications.Trim()).FirstOrDefault();
                }
                else
                    return specification;
            }
        }

        /// <summary>
        /// Property to set the Procedure textbox or combobox.
        /// </summary>
        public AcceptanceAsPer AcceptanceAsPers
        {
            get
            {
                AcceptanceAsPer acceptanceAsPer = new AcceptanceAsPer();
                var ctx = (RadiographyContext)this.DomainSource.DomainContext;
                ctx.Load(ctx.GetAcceptanceAsPersQuery());
                if (RGReport.AcceptanceAsPer != null)
                {
                    if (ctx.AcceptanceAsPers.Where(p => p.Value == RGReport.AcceptanceAsPer.Trim()).FirstOrDefault() == null)
                    {
                        cmbAcceptance.Visibility = Visibility.Collapsed;
                        txtAcceptance.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        cmbAcceptance.Visibility = Visibility.Visible;
                        txtAcceptance.Visibility = Visibility.Collapsed;
                    }
                    return ctx.AcceptanceAsPers.Where(p => p.Value == RGReport.AcceptanceAsPer.Trim()).FirstOrDefault();
                }
                else
                    return acceptanceAsPer;
            }
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            callClickEvent(row);
            TypeOfGrid = RGReportDataGrid;
        }

        private void btnCheckClerk_Click(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            callClickEvent(row);
            TypeOfGrid = RGReportDataGridClerk;
        }

        protected void callClickEvent(DataGridRow row)
        {
            RGReportRow reportRow = (RGReportRow)row.DataContext;
            DataGridRowForObservations = row;
            var observations = new AddObservations
            {
                MultipleObservations = reportRow.Observations,
                ReportID = reportRow.ID
            };

            observations.Show();
            observations.SubmitClicked += new EventHandler(AddObservations_SubmitClicked);
        }

        public string MultipleObservations { get; set; }
        public DataGridRow DataGridRowForObservations { get; set; }
        public CustomGrid TypeOfGrid { get; set; }

        void AddObservations_SubmitClicked(object sender, EventArgs e)
        {
            var window = sender as AddObservations;
            if (window != null && window.DialogResult == true)
            {
                MultipleObservations = window.MultipleObservations;
                TextBlock txtBlock = this.TypeOfGrid.Columns[12].GetCellContent(DataGridRowForObservations) as TextBlock;
                txtBlock.Text = MultipleObservations;
            }
        }
    }
}