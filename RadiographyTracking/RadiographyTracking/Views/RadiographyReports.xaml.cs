using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using BindableDataGrid.Data;
using RadiographyTracking.Web.Models;
using RadiographyTracking.Web.Services;
using Vagsons.Controls;
using System.Windows.Browser;

namespace RadiographyTracking.Views
{
    public partial class RadiographyReports : BaseCRUDView
    {
        RadiographyContext ctx;
        private BindableDataGrid.BindableDataGrid reportGrid;
        public RadiographyReports()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;

            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
            txtRTNo.Text = string.Empty;
            // cmbCoverage.SelectedValue = null;
        }

        public override string ChangeContext
        {
            get
            {
                return "Report No ";
            }
        }

        public override string ChangeContextProperty
        {
            get
            {
                return "ReportNo";
            }
        }


        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.RGDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.RGDomainDataSource; }
        }

        public override Type MainType
        {
            get { return typeof(RGReport); }
        }


        public void EditOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            App.RGReport = (RGReport)row.DataContext;
            Navigate("/EnterRadioGraphyReport");
        }

        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            App.RGReport = null;
            Navigate("/EnterRadioGraphyReport");
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            RGReport report = (RGReport)row.DataContext;
            if (!report.CanDelete)
            {
                MessageBox.Show("Can Delete only latest report for any RT");
                return;
            }

            base.DeleteOperation(sender, e);
            //save this, since there is no separate save button
            this.SaveOperation(sender, e);
        }


        public void PrintOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            RGReport report = (RGReport)row.DataContext;

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

            Uri reportURI = new Uri(string.Format(appRoot + "RGReportGenerate.aspx?ReportNo={0}", report.ReportNo), UriKind.Absolute);
            HtmlPage.Window.Navigate(reportURI, "_blank");
        }

        public void ExcelExportOperation(object sender, RoutedEventArgs e)
        {

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            RGReport report = (RGReport)row.DataContext;

            ctx = new RadiographyContext();
            busyIndicator.IsBusy = true;
            this.ctx.Load(this.ctx.GetRGReportsQuery(report.ReportNo)).Completed += loadCompleted; ;
            var str = "";





        }
        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;

            var operation = sender as LoadOperation<RGReport>;
            var rgReport = operation.Entities.FirstOrDefault();

            var reportTable = new DataTable("Report");
            var cols = reportTable.Columns;
            var rows = reportTable.Rows;

            var headerRow = new DataRow();
            rows.Add(headerRow);

            var columnNames = new List<string>()
               {"SlNo","Date","Location","Thickness","SFD","Designation","Sensitivity", 
                "Density", "FilmSize","Observations","Classifactions","Remarks", "ReportNo",
                "ReportDate", "Film","RTNO","FPNO","HEATNO","DateOfTest","LeadScreenFrontBack",
                "Viewing","Source","SourceSize","Coverage","ProcedureReference","AcceptanceAsper",
                "TotalNoOfFilms","Ir 192 Area","Co 60 Area","Ir 192 Strength","Co 60 Strenth"
               };

            foreach (var columnName in columnNames)
            {
                AddTextColumn(reportTable, columnName, columnName);
            }

            foreach (var columnName in columnNames )
            {
                headerRow[columnName] = columnName;
            }

            foreach (RGReportRow row in rgReport.RGReportRows)
            {
                var dataRow = new DataRow();
                dataRow["SlNo"] = rows.Count + 1;
                dataRow["Location"] = row.Location;
                dataRow["Thickness"] = row.Thickness;
                dataRow["SFD"] = row.SFD;
                dataRow["Designation"] = row.Designation;
                dataRow["Sensitivity"] = row.Sensitivity;
                dataRow["Density"] = row.Density;
                dataRow["FilmSize"] = row.FilmSizeString;
                dataRow["Observations"] = row.Observations;
                dataRow["Classifactions"] = row.Classifications;
                dataRow["Remarks"] = row.RemarkText;
                dataRow["ReportNo"] = rgReport.ReportNo;
                dataRow["ReportDate"] = rgReport.ReportDate;
                dataRow["Film"] = rgReport.Film;
                dataRow["RTNO"] = rgReport.RTNo;
                dataRow["FPNO"] = rgReport.FixedPatternID;
                dataRow["HEATNO"] = rgReport.HeatNo;
                dataRow["DateOfTest"] = rgReport.DateOfTest;
                dataRow["LeadScreenFrontBack"] = rgReport.LeadScreen;
                dataRow["Viewing"] = rgReport.Viewing;
                dataRow["Source"] = rgReport.Source;
                dataRow["SourceSize"] = rgReport.SourceSize;
                //dataRow["Coverage"] = rgReport.Coverage.CoverageName;
                dataRow["ProcedureReference"] = rgReport.ProcedureRef ?? string.Empty;
                dataRow["AcceptanceAsper"] = rgReport.AcceptanceAsPer ?? string.Empty;
                dataRow["TotalNoOfFilms"] = row.FilmCount;
                rows.Add(dataRow);
            }

            

            var ds = new DataSet("ReportDataSet");
            ds.Tables.Add(reportTable);

            reportGrid = new BindableDataGrid.BindableDataGrid();
            reportGrid.DataSource = ds;
            reportGrid.DataMember = "Report";
            reportGrid.DataBind();

           busyIndicator.IsBusy = false;


            MessageBox.Show("Report exported . Click on download button to download file !");
            btnDownload.Visibility=Visibility.Visible;

        }


        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            reportGrid.Export("Nithesh", "Gama", "", 0);
            btnDownload.Visibility = Visibility.Collapsed;
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

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
           RGDomainDataSource.Load();
        }
    }
}
