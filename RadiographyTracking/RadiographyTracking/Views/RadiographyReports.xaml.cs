﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.DomainServices.Client;
using System.Windows;
using System.Windows.Controls;
using BindableDataGrid.Data;
using RadiographyTracking.Web.Models;
using RadiographyTracking.Web.Services;
using Vagsons.Controls;
using System.Windows.Browser;
using System.Collections;

namespace RadiographyTracking.Views
{
    public partial class RadiographyReports : BaseCRUDView
    {
        RadiographyContext ctx;
        private BindableDataGrid.BindableDataGrid reportGrid;
        private string reportNumberToDownload;

        public RadiographyReports()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;

            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);// Convert.ToDateTime("03/03/2012"); 
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;// Convert.ToDateTime("03/23/2012"); 
            txtRTNo.Text = string.Empty;
            txtHeatNo.Text = string.Empty;

            if (WebContext.Current.User.Roles.FirstOrDefault() == "Customer")
            {
                customerMode = true;
                btnAdd.Visibility = Visibility.Collapsed;
                RGDataGrid.Visibility = Visibility.Collapsed;
                RGDataGridCustomer.Visibility = Visibility.Visible;
            }
            else
            {
                customerMode = false;
                btnAdd.Visibility = Visibility.Visible;
                RGDataGrid.Visibility = Visibility.Visible;
                RGDataGridCustomer.Visibility = Visibility.Collapsed;
            }
            if (WebContext.Current.User.Roles.FirstOrDefault() == "Admin")
            {
                txtFilmStock.Visibility = Visibility.Visible;
                cmbFoundry.Visibility = Visibility.Visible;
            }
            else
            {
                txtFilmStock.Visibility = Visibility.Collapsed;
                cmbFoundry.Visibility = Visibility.Collapsed;
            }
        }


        private bool customerMode = false;

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
            //get { return this.RGDataGrid; }
            get { return customerMode ? RGDataGridCustomer : RGDataGrid; }
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
            if (WebContext.Current.User.Roles.FirstOrDefault() == "Customer")
                return;

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
            if (WebContext.Current.User.Roles.FirstOrDefault() == "Customer")
                return;

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

            CheckBox cmb = null;
            if (WebContext.Current.User.Roles.FirstOrDefault() == "Customer")
                cmb = this.RGDataGridCustomer.Columns[7].GetCellContent(row) as CheckBox;
            else
                cmb = this.RGDataGrid.Columns[9].GetCellContent(row) as CheckBox;

            //Get the root path for the XAP
            string src = Application.Current.Host.Source.ToString();

            //Get the application root, where 'ClientBin' is the known dir where the XAP is
            string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

            //Uri reportURI = new Uri(string.Format(appRoot + "RGReportGenerate.aspx?ReportNo={0}", report.ReportNo), UriKind.Absolute);
            Debug.Assert(report != null, "report != null");
            Uri reportURI = new Uri(string.Format(appRoot + "RGReportGenerate.aspx?ReportId={0}&" + "ReportNo={1}&FilmSize={2}", report.ID, report.ReportNo, cmb.IsChecked), UriKind.Absolute);
            HtmlPage.Window.Navigate(reportURI, "_blank");
        }

        public void ExcelExportOperation(object sender, RoutedEventArgs e)
        {
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            RGReport report = (RGReport)row.DataContext;

            CheckBox cmb = null;
            if (WebContext.Current.User.Roles.FirstOrDefault() == "Customer")
                cmb = this.RGDataGridCustomer.Columns[7].GetCellContent(row) as CheckBox;
            else
                cmb = this.RGDataGrid.Columns[9].GetCellContent(row) as CheckBox;

            ctx = (RadiographyContext)RGDomainDataSource.DomainContext;
            busyIndicator.IsBusy = true;
            IsFilmSizeInCms = cmb.IsChecked.Value;
            this.ctx.Load(this.ctx.GetRGReportsQuery(report.ID)).Completed += loadCompleted;

        }

        public bool IsFilmSizeInCms { get; set; }
        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;

            var operation = sender as LoadOperation<RGReport>;
            var rgReport = operation.Entities.FirstOrDefault();
            reportNumberToDownload = rgReport.ReportNo;

            var reportTable = new DataTable("Report");
            var cols = reportTable.Columns;
            var rows = reportTable.Rows;

            var headerRow = new DataRow();
            rows.Add(headerRow);

            var columnNames = new List<string>()
               {"SlNo","Location","Thickness","SFD","IQI Designation","IQI Sensitivity", 
                "Density", "FilmSize","Observations","Classifactions","Remarks", "ReportNo",
                "ReportDate", "Film","RTNO","FPNO","HEATNO","DateOfTest","LeadScreenFront","LeadScreenBack",
                "Viewing","Source","SourceSize","Coverage","ProcedureReference","AcceptanceAsper",
                "TotalNoOfFilms"
               };

            foreach (var en in ctx.Energies)
            {
                columnNames.Add(en.Name + " Area");
            }

            columnNames.Add("Ir192_Co60 Strength");

            foreach (var columnName in columnNames)
            {
                AddTextColumn(reportTable, columnName, columnName);
            }

            foreach (var columnName in columnNames)
            {
                headerRow[columnName] = columnName;
            }

            var totalFilmCount = rgReport.RGReportRows.Sum(p => p.FilmCount);

            foreach (RGReportRow row in rgReport.RGReportRows)
            {
                var dataRow = new DataRow();
                dataRow["SlNo"] = rows.Count;
                dataRow["Location"] = row.LocationAndSegment ?? string.Empty;
                dataRow["Thickness"] = row.ThicknessRange;
                dataRow["SFD"] = row.SFD;
                dataRow["IQI Designation"] = row.Designation ?? string.Empty;
                dataRow["IQI Sensitivity"] = row.Sensitivity ?? string.Empty;
                dataRow["Density"] = row.Density ?? string.Empty;
                if (!IsFilmSizeInCms)
                    dataRow["FilmSize"] = row.FilmSizeString ?? string.Empty;
                else
                    dataRow["FilmSize"] = row.FilmSizeStringInCms ?? string.Empty;
                dataRow["Observations"] = row.Findings ?? string.Empty;
                dataRow["Classifactions"] = row.Classifications ?? string.Empty;
                dataRow["Remarks"] = row.RemarkText ?? string.Empty;
                dataRow["ReportNo"] = rgReport.ReportNo ?? string.Empty;
                dataRow["ReportDate"] = rgReport.ReportDate.ToShortDateString();
                dataRow["Film"] = rgReport.Film ?? string.Empty;
                dataRow["RTNO"] = rgReport.RTNo ?? string.Empty;
                dataRow["FPNO"] = rgReport.FixedPattern != null ? rgReport.FixedPattern.FPNo : string.Empty;
                dataRow["HEATNO"] = rgReport.HeatNo ?? string.Empty;
                dataRow["DateOfTest"] = rgReport.DateOfTest.ToShortDateString();
                dataRow["LeadScreenFront"] = rgReport.LeadScreen;
                dataRow["LeadScreenBack"] = rgReport.LeadScreenBack;
                dataRow["Viewing"] = rgReport.Viewing ?? string.Empty;
                dataRow["Source"] = rgReport.Source ?? string.Empty;
                dataRow["SourceSize"] = rgReport.SourceSize ?? string.Empty;
                dataRow["Coverage"] = rgReport.Coverage != null ? rgReport.Coverage.CoverageName : string.Empty;
                dataRow["ProcedureReference"] = rgReport.ProcedureRef ?? string.Empty;
                dataRow["AcceptanceAsper"] = rgReport.AcceptanceAsPer ?? string.Empty;
                dataRow["TotalNoOfFilms"] = row.FilmCount;    //totalFilmCount;
                dataRow["Ir192_Co60 Strength"] = rgReport.Strength ?? string.Empty;

                foreach (var en in ctx.Energies)
                {
                    if (!IsFilmSizeInCms)
                    {
                        dataRow[en.Name + " Area"] = rgReport.RGReportRows
                            .Where(p => p.EnergyID == en.ID &&
                                //  p.RemarkText != "RETAKE" &&
                                        p.SlNo == rows.Count)
                            .Sum(p => p.FilmSize.Area * p.FilmCount);
                    }
                    else
                    {
                        dataRow[en.Name + " Area"] = rgReport.RGReportRows
                           .Where(p => p.EnergyID == en.ID &&
                               //  p.RemarkText != "RETAKE" &&
                                       p.SlNo == rows.Count)
                           .Sum(p => p.FilmSize.AreaInCms * p.FilmCount);
                    }
                }

                rows.Add(dataRow);
            }



            var ds = new DataSet("ReportDataSet");
            ds.Tables.Add(reportTable);

            reportGrid = new BindableDataGrid.BindableDataGrid();
            reportGrid.DataSource = ds;
            reportGrid.DataMember = "Report";
            reportGrid.DataBind();

            busyIndicator.IsBusy = false;

            myPopup.IsOpen = true;

        }


        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
            reportGrid.Export("Nithesh", "Gama", "", 1, "Report No " + reportNumberToDownload);
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

        protected string GetTechniqueWithComma(ICollection<RGReportRow> FinalRTReportRows)
        {
            return string.Join(",", FinalRTReportRows.Select(p => p.Technique).Distinct()); ;
        }

        private void CmbFoundry_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fromDatePicker.Text = null;
            toDatePicker.Text = null;
            txtRTNo.Text = string.Empty;
            txtHeatNo.Text = string.Empty;
            cmbCoverage.SelectedItem = null;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFetch.IsEnabled = ValueChanged();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            btnFetch.IsEnabled = ValueChanged();
        }

        public bool ValueChanged()
        {
            return (!(String.IsNullOrEmpty(fromDatePicker.Text) ||
                      String.IsNullOrEmpty(toDatePicker.Text))
                      || !((String.IsNullOrEmpty(txtRTNo.Text) &&
                                                            String.IsNullOrEmpty(txtHeatNo.Text)))
                      );
        }
    }
}
