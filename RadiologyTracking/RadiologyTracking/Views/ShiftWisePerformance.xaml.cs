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
using RadiologyTracking.Web.Services;
using System.Threading;
using RadiologyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;
using System.Collections;
using RadiologyTracking.Controls;
using System.Windows.Data;


namespace RadiologyTracking.Views
{
    public partial class ShiftWisePerformance : BaseCRUDView
    {
        RadiologyContext ctx;
        DataTable reportTable;
        public ShiftWisePerformance()
            : base()
        {
            InitializeComponent();
            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiologyContext();
            busyIndicator.IsBusy = true;            
            ctx.Load(ctx.GetFilmSizesQuery()).Completed += filmSizeLoaded;         
        }

        private void filmSizeLoaded(object sender, EventArgs e)
        {
            int technicianId = (cmbTechnicians.SelectedIndex == -1 ? -1 : ((Technician)cmbTechnicians.SelectedItem).ID);
            ctx.Load(ctx.GetShiftWisePerformanceReportQuery((DateTime)fromDatePicker.SelectedDate,
                    (DateTime)toDatePicker.SelectedDate, technicianId)).Completed += loadCompleted;   
        }

        private void loadCompleted(object sender, EventArgs e)
        {
            //wait till fully loaded
            if (ctx.IsLoading) return;

            var report = ((LoadOperation<ShiftWisePerformanceRow>)sender).Entities;         

            //Can't directly bind the report to the grid due to variable no of columns. Using a custom datagrid bindable
            //to a datatable instead

            reportTable = new DataTable("Report");
            var cols = reportTable.Columns;
            var rows = reportTable.Rows;

            DataRow headerRow = new DataRow();
            rows.Add(headerRow);

            DataRow subHeaderRow = new DataRow();
            rows.Add(subHeaderRow);
                        
            AddTextColumn(reportTable, "Technicians", "Technicians");
            AddTextColumn(reportTable, "Date", "Date");
            AddTextColumn(reportTable, "Shift", "Shift");

            headerRow["Technicians"] = "Technicians";
            headerRow["Date"] = "Date";
            headerRow["Shift"] = "Shift";

            subHeaderRow["Technicians"] = "";
            subHeaderRow["Date"] = "";
            subHeaderRow["Shift"] = "";

            //filmsize columns
            foreach (var row in ctx.FilmSizes.OrderBy(p => p.Area))
            {
                var totalcol = "Film" + row.Name + "Total";
                var rtcol = "Film" + row.Name + "RT";
                AddTextColumn(reportTable, totalcol, totalcol);
                AddTextColumn(reportTable, rtcol, rtcol);
                headerRow[totalcol] = row.Name;
                headerRow[rtcol] = "";
                subHeaderRow[totalcol] = "Total";
                subHeaderRow[rtcol] = "RT";
            }

            AddTextColumn(reportTable, "TotalFilms", "Total Films Taken");
            AddTextColumn(reportTable, "TotalRetakes", "Total Retakes");
            AddTextColumn(reportTable, "RTPercent", "RT % by No");
            AddTextColumn(reportTable, "RTPercentByArea", "RT % by Area");

            headerRow["TotalFilms"] = "Total Films Taken";
            headerRow["TotalRetakes"] = "Total Retakes";
            headerRow["RTPercent"] = "RT % by No";
            headerRow["RTPercentByArea"] = "RT % by Area";

            subHeaderRow["TotalFilms"] = "";
            subHeaderRow["TotalRetakes"] = "";
            subHeaderRow["RTPercent"] = "";
            subHeaderRow["RTPercentByArea"] = "";

            foreach (var r in report)
            {
                DataRow row = new DataRow();
                row["Technicians"] = r.Technicians;
                row["Date"] = r.Date;
                row["Shift"] = r.Shift;

                foreach(var energy in r.FilmAreaRows)
                {
                    row["Film" + energy.FilmSize + "Total"] = energy.Total;
                    row["Film" + energy.FilmSize + "RT"] = energy.RT;
                }

                row["TotalFilms"] = r.TotalFilmsTaken;
                row["TotalRetakes"] = r.TotalRetakes;
                row["RTPercent"] = r.RTPercent;
                row["RTPercentByArea"] = r.RTPercentByArea;
                rows.Add(row);
            }

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
            //identify cells to be merged
            String mergeCells = "";
            var row = reportTable.Rows[0];
            var columns = reportTable.Columns;
            int mergeCounter = 0;

            for (int i = 0; i < columns.Count; i++ )
            {
                string colName = columns[i].ColumnName;
                if (!row.Items.ContainsKey(colName))
                    continue;

                string cellValue = row[colName].ToString();
                //till first cell with content
                if (mergeCells == "" && cellValue == "")
                    continue;

                //first cell with content
                else if (mergeCells == "" && cellValue != "")
                    mergeCells = mergeCells + "0-" + i.ToString() + "-";

                //increment merge count till hit next cell with content
                else if (mergeCells != "" && cellValue == "")
                    mergeCounter ++;

                else if (mergeCells != "" && cellValue != "")
                {
                    mergeCells = mergeCells + mergeCounter.ToString() + "," + "0-"+ i.ToString() + "-";
                    //reset merge counter
                    mergeCounter = 0;
                }
            }

            //for last merge cell
            mergeCells = mergeCells + mergeCounter.ToString();            
            reportGrid.Export("Roopesh", "Gama", mergeCells, 2);
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnFetch.IsEnabled = !(String.IsNullOrEmpty(fromDatePicker.Text) || String.IsNullOrEmpty(toDatePicker.Text));
        }
    }
}
