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
    public partial class FixedPatternPerformance : BaseCRUDView
    {
        RadiographyContext ctx;
        DataTable reportTable;
        public FixedPatternPerformance()
            : base()
        {
            InitializeComponent();
        }

        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiographyContext();
            busyIndicator.IsBusy = true;
            ctx.Load(ctx.GetFixedPatternPerformanceReportQuery(txtFPNo.Text, (bool)chkLike.IsChecked)).Completed += loadCompleted;
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

            AddTextColumn(reportTable, "FPNo", "FP No");
            AddTextColumn(reportTable, "RTNo", "RT No");
            AddTextColumn(reportTable, "ReportNo", "Report No");
            AddTextColumn(reportTable, "Coverage", "Coverage");
            AddTextColumn(reportTable, "HeatNo", "Heat No");
            AddTextColumn(reportTable, "Date", "Date");

            segmentRow["FPNo"] = "FPNo";
            segmentRow["ReportNo"] = "ReportNo";
            segmentRow["RTNo"] = "RTNo";
            segmentRow["HeatNo"] = "HeatNo";
            segmentRow["Coverage"] = "Coverage";
            segmentRow["Date"] = "Date";

            string prevRTNo = "";

            string prevLocn = "";
            foreach (var locseg in report
                                    .SelectMany(p => p.Locations)
                                    .SelectMany(l => l.Segments.Select(s => new Tuple<string, string>(l.Location, s.Segment)))
                                    .Distinct()
                                    .OrderBy(q => q.Item1).ThenBy(q => q.Item2))
            {
                string header = String.Concat(locseg.Item1, "-", locseg.Item2);
                string colname = "col" + header.Replace("-", "");
                if (cols.FirstOrDefault(p => p.Caption == header) == null)
                {
                    AddTextColumn(reportTable, colname, header);
                    locationRow[colname] = locseg.Item1 == prevLocn ? "" : locseg.Item1; //do not get location get repeated
                    prevLocn = locseg.Item1;
                    segmentRow[colname] = locseg.Item2;
                }
            }

            foreach (var rt in report)
            {
                DataRow row = new DataRow();
                row["FPNo"] = rt.FPNo; //txtFPNo.Text; //won't change throughout the report
                row["RTNo"] = prevRTNo == rt.RTNo ? "" : rt.RTNo; //prevent same RT No from repeating
                row["ReportNo"] = rt.ReportNo;
                row["Coverage"] = rt.Coverage;
                row["HeatNo"] = rt.HeatNo;
                prevRTNo = rt.RTNo;
                row["Date"] = rt.Date.ToString("dd/MM/yyyy");

                prevLocn = "";
                foreach (var loc in rt.Locations)
                {
                    foreach (var seg in loc.Segments)
                    {
                        string header = String.Concat(loc.Location, "-", seg.Segment);
                        string colname = "col" + header.Replace("-", "");
                        if (cols.FirstOrDefault(p => p.Caption == header) == null)
                        {
                            AddTextColumn(reportTable, colname, header);
                            locationRow[colname] = loc.Location == prevLocn ? "" : loc.Location; //do not get location get repeated
                            prevLocn = loc.Location;
                            segmentRow[colname] = seg.Segment;
                        }
                        //no need to show NSD - not any more
                        row[colname] = seg.Observations.ToUpper(); //.Replace("NSD", "");
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
            var row = reportTable.Rows[0];
            var columns = reportTable.Columns;
            String mergeCells = getMergeCells(row, columns);
            reportGrid.Export("Roopesh", "Gama", mergeCells, 2);
        }
    }
}