using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Services;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;


namespace RadiographyTracking.Views
{
    public partial class FilmConsumptionReport : BaseCRUDView
    {        
        DataTable reportTable;
        RadiographyContext ctx;
        string[] rowTypes = new string[] { "ACCEPTABLE", "REPAIR", "RESHOOT", "RETAKE" };
        

        public FilmConsumptionReport()
            : base()
        {
            InitializeComponent();
            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiographyContext();
            busyIndicator.IsBusy = true;
            
            ctx.Load(ctx.GetEnergiesQuery()).Completed += new EventHandler(FilmConsumptionReport_Completed);
        }

        void FilmConsumptionReport_Completed(object sender, EventArgs e)
        {
            int foundryId = cmbFoundry.SelectedIndex == -1 ? -1 : ((Foundry)cmbFoundry.SelectedItem).ID;
            ctx.Load(ctx.GetFilmConsumptionReportQuery(foundryId, (DateTime)fromDatePicker.SelectedDate,
                (DateTime)toDatePicker.SelectedDate)).Completed += loadCompleted;
        }

        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;

            var report = ((LoadOperation<FilmConsumptionReportRow>)sender).Entities;

            //Can't directly bind the report to the grid due to variable no of columns. Using a custom datagrid bindable
            //to a datatable instead

            reportTable = new DataTable("Report");
            var cols = reportTable.Columns;
            var rows = reportTable.Rows;

            var headerRow = new DataRow();
            rows.Add(headerRow);

            var subHeaderRow = new DataRow();
            rows.Add(subHeaderRow);

            AddTextColumn(reportTable, "ReportNo", "Report No");
            AddTextColumn(reportTable, "ReportDate", "Report Date");
            AddTextColumn(reportTable, "FPNo", "FP No");
            AddTextColumn(reportTable, "RTNo", "RT No");
            AddTextColumn(reportTable, "ReportTypeAndNo", "Remarks");

            headerRow["ReportNo"] = "Report No";
            headerRow["ReportDate"] = "Date";
            headerRow["FPNo"] = "FP No";
            headerRow["RTNo"] = "RT No";
            headerRow["ReportTypeAndNo"] = "Remarks";

            subHeaderRow["ReportNo"] = "";
            subHeaderRow["ReportDate"] = "";
            subHeaderRow["FPNo"] = "";
            subHeaderRow["RTNo"] = "";
            subHeaderRow["ReportTypeAndNo"] = "";

            //filmsize columns
            foreach (var row in ctx.Energies)
            {
                foreach (var type in rowTypes)
                {
                    var colName = String.Format("{0}{1}", row.Name, type);
                    AddTextColumn(reportTable, colName, colName);
                    headerRow[colName] = type == "ACCEPTABLE" ? row.Name : string.Empty;
                    subHeaderRow[colName] = type == "ACCEPTABLE" ? "T" : type == "REPAIR" ? "RP" : type == "RETAKE" ? "RT" : "RS";
                }
            }

            string prevReportNo = "";

            DataRow dataRow = null;

            float totalArea = 0;

            string prevEnergy = String.Empty;
            int rowCount = 1;

            //the loop pivots the data wrt to energy and row types
            foreach (var r in report)
            {
                if(r.ReportNo != prevReportNo)
                {
                    if (dataRow != null)
                    {
                        //prev Row last energy total col - temp fix for issue 0000109
                        dataRow[prevEnergy + "ACCEPTABLE"] = totalArea;
                        totalArea = 0;
                        rows.Add(dataRow);
                    }
                    dataRow = new DataRow();
                    dataRow["ReportNo"] = prevReportNo = r.ReportNo; //set prevReportNo for next time
                    dataRow["ReportDate"] = r.Date;
                    dataRow["FPNo"] = r.FPNo;
                    dataRow["RTNo"] = r.RTNo;
                    dataRow["ReportTypeAndNo"] = r.ReportTypeAndNo;
                }

                //prev Row last energy total col - temp fix for issue 0000109
                if (r.Energy != prevEnergy)
                {
                    dataRow[prevEnergy + "ACCEPTABLE"] = totalArea;
                    totalArea = 0;
                }

                dataRow[r.Energy + r.RowType] = r.Area;
                totalArea += r.Area;

                prevEnergy = r.Energy;
            }
            //last report row
            if(dataRow != null) rows.Add(dataRow);

            //totals row
            var totalRow = new DataRow();

            
            foreach (var row in ctx.Energies)
            {
                foreach (var type in rowTypes)
                {
                    var colName = String.Format("{0}{1}", row.Name, type);
                    totalRow[colName] = rows.Select(p => (p[colName] as float?) ?? 0).Sum();
                }
            }

            rows.Add(totalRow);

            var ds = new DataSet("ReportDataSet");
            ds.Tables.Add(reportTable);

            reportGrid.DataSource = ds;
            reportGrid.DataMember = "Report";
            reportGrid.DataBind();

            busyIndicator.IsBusy = false;

        }

        private static void AddTextColumn(DataTable reportTable, String columnName, String caption)
        {
            var dc = new DataColumn(columnName);
            dc.Caption = caption;
            dc.ReadOnly = true;
            dc.DataType = typeof(String);
            dc.AllowResize = true;
            dc.AllowSort = false;
            dc.AllowReorder = false;
            reportTable.Columns.Add(dc);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var row = reportTable.Rows[0];
            var columns = reportTable.Columns;
            String mergeCells = getMergeCells(row, columns);
            reportGrid.Export("Roopesh", "Gama", mergeCells, 2);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFetch.IsEnabled = !(cmbFoundry.SelectedIndex == -1 || String.IsNullOrEmpty(fromDatePicker.Text) || 
                                  String.IsNullOrEmpty(toDatePicker.Text));
        }
    }
}