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
using System.Threading;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;
using System.Collections;
using RadiographyTracking.Controls;
using System.Windows.Data;


namespace RadiographyTracking.Views
{
    public partial class FilmConsumptionReport : BaseCRUDView
    {        
        DataTable reportTable;
        RadiographyContext ctx;

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

            DataRow headerRow = new DataRow();
            rows.Add(headerRow);

            DataRow subHeaderRow = new DataRow();
            rows.Add(subHeaderRow);

            AddTextColumn(reportTable, "ReportNo", "Report No");
            AddTextColumn(reportTable, "ReportDate", "Report Date");
            AddTextColumn(reportTable, "FPNo", "FP No");
            AddTextColumn(reportTable, "RTNo", "RT No");

            headerRow["ReportNo"] = "Report No";
            headerRow["ReportDate"] = "Date";
            headerRow["FPNo"] = "FP No";
            headerRow["RTNo"] = "RT No";

            subHeaderRow["ReportNo"] = "";
            subHeaderRow["ReportDate"] = "";
            subHeaderRow["FPNo"] = "";
            subHeaderRow["RTNo"] = "";

            //filmsize columns
            foreach (var row in ctx.Energies)
            {
                var freshCol = row.Name + "FRESH";
                var repairCol = row.Name + "REPAIR";
                var reshootCol = row.Name + "RESHOOT";
                AddTextColumn(reportTable, freshCol, freshCol);
                AddTextColumn(reportTable, repairCol, repairCol);
                AddTextColumn(reportTable, reshootCol, reshootCol);
                headerRow[freshCol] = row.Name;
                headerRow[repairCol] = "";
                headerRow[reshootCol] = "";
                subHeaderRow[freshCol] = "F";
                subHeaderRow[repairCol] = "RP";
                subHeaderRow[reshootCol] = "RS";
            }

            string prevReportNo = "";

            DataRow dataRow = null;

            //the loop pivots the data wrt to energy and row types
            foreach (var r in report)
            {
                if(r.ReportNo != prevReportNo)
                {
                    if(dataRow != null) rows.Add(dataRow);
                    dataRow = new DataRow();
                    dataRow["ReportNo"] = prevReportNo = r.ReportNo; //set prevReportNo for next time
                    dataRow["ReportDate"] = r.Date;
                    dataRow["FPNo"] = r.FPNo;
                    dataRow["RTNo"] = r.RTNo;                    
                }
                dataRow[r.Energy + r.RowType] = r.Area;                
            }
            //last report row
            if(dataRow != null) rows.Add(dataRow);

            //totals row
            DataRow totalRow = new DataRow();
            foreach (var row in ctx.Energies)
            {
                var freshCol = row.Name + "FRESH";
                var repairCol = row.Name + "REPAIR";
                var reshootCol = row.Name + "RESHOOT";
                totalRow[freshCol] = rows.Select(p => (p[freshCol] as int?) ?? 0 ).Sum();
                totalRow[repairCol] = rows.Select(p => (p[repairCol] as int?) ?? 0).Sum();
                totalRow[reshootCol] = rows.Select(p => (p[reshootCol] as int?) ?? 0).Sum();
            }

            rows.Add(totalRow);

            DataSet ds = new DataSet("ReportDataSet");
            ds.Tables.Add(reportTable);

            reportGrid.DataSource = ds;
            reportGrid.DataMember = "Report";
            reportGrid.DataBind();

            busyIndicator.IsBusy = false;

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

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            reportGrid.Export("Roopesh", "Gama", "", 0);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFetch.IsEnabled = !(cmbFoundry.SelectedIndex == -1 || String.IsNullOrEmpty(fromDatePicker.Text) || 
                                  String.IsNullOrEmpty(toDatePicker.Text));
        }
    }
}