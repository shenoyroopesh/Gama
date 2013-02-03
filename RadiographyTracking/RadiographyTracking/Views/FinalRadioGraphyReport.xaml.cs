using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Models;
using Vagsons.Controls;
using System.Windows.Data;
using RadiographyTracking.Web.Services;
using BindableDataGrid.Data;
using System.Windows.Browser;

namespace RadiographyTracking.Views
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
            BindToPage(txtSource, TextBlock.TextProperty, "FinalReport.Source");
            BindToPage(txtStrength, TextBlock.TextProperty, "FinalReport.Strength");
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
            BindToPage(lblTotalArea, TextBlock.TextProperty, "FinalReport.TotalArea");
            BindToPage(cmbSelectTemplate, ComboBox.ItemsSourceProperty, "FinalReport.ReportTemplatesList");
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
            updateEnergyWiseArea();
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

        private void FilmSizeArea_LoadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //just in case it din't get loaded earlier
            updateEnergyWiseArea();
        }

        public void updateEnergyWiseArea()
        {
            RadiographyContext ctx = (RadiographyContext)this.DomainSource.DomainContext;
            DataTable dt = new DataTable("EnergyTable");
            AddTextColumn(dt, "HeadRow", "HeadRow");
            DataRow headerRow = new DataRow();
            DataRow actualRow = new DataRow();
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
                actualRow[e.Name] = FinalReportRows
                                            .Where(p => p.EnergyID == e.ID &&
                                                   p.RemarkText != "RETAKE") //30-Jun-12 - Roopesh added this to ensure that retake areas are not included
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
            DataColumn dc = new DataColumn(columnName);
            dc.Caption = caption;
            dc.ReadOnly = true;
            dc.DataType = typeof(String);
            dc.AllowResize = true;
            dc.AllowSort = false;
            dc.AllowReorder = false;
            reportTable.Columns.Add(dc);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSelectTemplate.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a report template");
                return;
            }

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin")); 

            Uri reportURI = new Uri(string.Format(appRoot + "FinalRGReportGenerate.aspx?RTNo={0}&Template={1}&Filter={2}",
                                                    this.FinalReport.RTNo, cmbSelectTemplate.SelectedValue.ToString(), ((bool)chkOnlyRepairs.IsChecked ? "True" : "False")),
                                    UriKind.Absolute);

            HtmlPage.Window.Navigate(reportURI, "_blank");
        }
    }
}