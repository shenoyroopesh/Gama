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
    public partial class FixedPatternPerformance : BaseCRUDView
    {
        RadiologyContext ctx;
        DataTable reportTable;
        public FixedPatternPerformance()
            : base()
        {
            InitializeComponent();
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiologyContext();
            busyIndicator.IsBusy = true;
            ctx.Load(ctx.GetFixedPatternPerformanceReportQuery(txtFPNo.Text)).Completed += loadCompleted;
        }

        private void loadCompleted(object sender, EventArgs e)
        {
            var report = ((LoadOperation<FixedPatternPerformanceRow>)sender).Entities;         

            //Can't directly bind the report to the grid due to variable no of columns. Using a custom datagrid bindable
            //to a datatable instead

            reportTable = new DataTable("Report");
            var cols = reportTable.Columns;
            var rows = reportTable.Rows;

            DataRow locationRow = new DataRow();
            rows.Add(locationRow);

            DataRow segmentRow = new DataRow();
            rows.Add(segmentRow);
            
            AddTextColumn(reportTable, "RTNo", "RT No");
            AddTextColumn(reportTable, "Date", "Date");

            string prevRTNo = "";
            foreach (var rt in report)
            {
                DataRow row = new DataRow();
                row["RTNo"] = prevRTNo == rt.RTNo ? "" : rt.RTNo; //prevent same RT No from repeating
                prevRTNo = rt.RTNo;
                segmentRow["RTNo"] = "RTNo";
                row["Date"] = rt.Date.ToString("dd/MM/yyyy");
                segmentRow["Date"] = "Date";

                string prevLocn = "";
                foreach (var loc in rt.Locations)
                {
                    
                    foreach (var seg in loc.Segments)
                    {
                        string header = String.Concat(loc.Location, "-", seg.Segment);
                        string colname = header.Replace("-", "");
                        if (cols.FirstOrDefault(p => p.Caption == header) == null)
                        {
                            AddTextColumn(reportTable, colname, header);
                            locationRow[colname] = loc.Location == prevLocn? "" : loc.Location; //do not get location get repeated
                            prevLocn = loc.Location;

                            segmentRow[colname] = seg.Segment;                            
                        }
                        //no need to show NSD
                        row[colname] = seg.Observations.ToUpper().Replace("NSD", "");
                    }
                }
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
            dc.AllowSort = true;
            dc.AllowReorder = true;
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
    }
}
