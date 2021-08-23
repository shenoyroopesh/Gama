using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Services;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using BindableDataGrid.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.ServiceModel;

namespace RadiographyTracking.Views
{
    public partial class CastingHistoryReport : BaseCRUDView
    {
        DataTable reportTable;
        RadiographyContext ctx;
        string[] rowTypes = new string[] { "ACCEPTABLE", "REPAIR", "RESHOOT", "RETAKE" };

        public CastingHistoryReport()
            : base()
        {
            InitializeComponent();
        }


        private void btnFetch_Click(object sender, RoutedEventArgs e)
        {
            ctx = new RadiographyContext();
            busyIndicator.IsBusy = true;

            ctx.Load(ctx.GetEnergiesQuery()).Completed += new EventHandler(CastingHistoryReport_Completed);
        }

        void CastingHistoryReport_Completed(object sender, EventArgs e)
        {
            int foundryId = cmbFoundry.SelectedIndex == -1 ? -1 : ((Foundry)cmbFoundry.SelectedItem).ID;
            var coverageId = cmbCoverage.SelectedValue == null ? -1 : ((Coverage)cmbCoverage.SelectedValue).ID;

            List<FilmConsumptionReportRow> FilmConsumptionReportRowList = new List<FilmConsumptionReportRow>();

            EntityQuery<FilmConsumptionReportRow> query = ctx.GetCastingHistoryReportQuery(foundryId, txtRTNo.Text,
                txtHeatNo.Text, txtFPNo.Text, coverageId);

            LoadOperation<FilmConsumptionReportRow> loadOp = ctx.Load(query, loadOpN =>
            {
                foreach (var filmConsumptionReportRow in loadOpN.Entities)
                {
                    FilmConsumptionReportRowList.Add(filmConsumptionReportRow);
                }
                if (FilmConsumptionReportRowList.Count > 0)
                {
                    reportTable = new DataTable("Report");
                    var cols = reportTable.Columns;
                    var rows = reportTable.Rows;

                    var headerRow = new DataRow();
                    rows.Add(headerRow);

                    var subHeaderRow = new DataRow();
                    rows.Add(subHeaderRow);

                    AddTextColumn(reportTable, "ReportNo", "Report No");
                    AddTextColumn(reportTable, "ReportDate", "Report Date");
                    AddTextColumn(reportTable, "DateOfTest", "Date of Test");
                    AddTextColumn(reportTable, "RTNo", "RT No");

                    headerRow["ReportNo"] = "Report No";
                    headerRow["ReportDate"] = "Report Date";
                    headerRow["DateOfTest"] = "Date of Test";
                    headerRow["RTNo"] = "RT No";

                    subHeaderRow["ReportNo"] = "";
                    subHeaderRow["ReportDate"] = "";
                    subHeaderRow["DateOfTest"] = "";
                    subHeaderRow["RTNo"] = "";

                    int highestNoOfRepair = FilmConsumptionReportRowList.Select(p => p.ReshootNo).Max();

                    int rowIndexForFresh = 0;
                    foreach (var row in ctx.Energies)
                    {
                        var colName = "Fresh" + row.Name;
                        headerRow[colName] = rowIndexForFresh == 0 ? "Fresh " : string.Empty;
                        AddTextColumn(reportTable, colName, colName);
                        subHeaderRow[colName] = row.Name;
                        rowIndexForFresh++;
                    }

                    //Repair columns
                    for (int i = 0; i < highestNoOfRepair; i++)
                    {
                        int rowIndex = 0;
                        foreach (var row in ctx.Energies)
                        {
                            var colName = "Repair" + (i + 1) + row.Name;
                            AddTextColumn(reportTable, colName, colName);
                            headerRow[colName] = rowIndex == 0 ? "Repair " + (i + 1) : string.Empty;
                            subHeaderRow[colName] = row.Name;
                            rowIndex++;
                        }
                    }

                    //Reshoot columns
                    for (int i = 0; i < highestNoOfRepair; i++)
                    {
                        int rowIndex = 0;
                        foreach (var row in ctx.Energies)
                        {
                            var colName = "Reshoot" + (i + 1) + row.Name;
                            AddTextColumn(reportTable, colName, colName);
                            headerRow[colName] = rowIndex == 0 ? "Reshoot " + (i + 1) : string.Empty;
                            subHeaderRow[colName] = row.Name;
                            rowIndex++;
                        }
                    }

                    //Retake columns
                    for (int i = 0; i < highestNoOfRepair; i++)
                    {
                        int rowIndex = 0;
                        foreach (var row in ctx.Energies)
                        {
                            var colName = "Retake" + (i + 1) + row.Name;
                            AddTextColumn(reportTable, colName, colName);
                            headerRow[colName] = rowIndex == 0 ? "Retake " + (i + 1) : string.Empty;
                            subHeaderRow[colName] = row.Name;
                            rowIndex++;
                        }
                    }

                    //Retake columns
                    for (int i = 0; i < highestNoOfRepair; i++)
                    {
                        int rowIndex = 0;
                        foreach (var row in ctx.Energies)
                        {
                            var colName = "Checkshot" + (i + 1) + row.Name;
                            AddTextColumn(reportTable, colName, colName);
                            headerRow[colName] = rowIndex == 0 ? "Checkshot " + (i + 1) : string.Empty;
                            subHeaderRow[colName] = row.Name;
                            rowIndex++;
                        }
                    }
                    int countForInnerLoop = 0;
                    int noOfInnerLoops = FilmConsumptionReportRowList.Where(p => p.ReshootNo != 0).Count();
                    List<RGReport> rgReportList = new List<RGReport>();

                    foreach (var filmConsumptionReportRow in FilmConsumptionReportRowList)
                    {
                        if (filmConsumptionReportRow.ReshootNo == 0)
                        {
                        }
                        else
                        {
                            EntityQuery<RGReport> queryInner = ctx.GetRGReportsOnRtNoAndReshootNoForReportQuery(filmConsumptionReportRow.RTNo,
                            (filmConsumptionReportRow.ReshootNo - 1));

                            LoadOperation<RGReport> loadOpInner = ctx.Load(queryInner, loadOpNInner =>
                            {
                                countForInnerLoop++;
                                foreach (var rgReport in loadOpNInner.Entities)
                                {
                                    rgReportList.Add(rgReport);
                                }

                                if (noOfInnerLoops == countForInnerLoop)
                                {
                                    foreach (var filmConsumptionReportRowInner in FilmConsumptionReportRowList)
                                    {
                                        string prevReportNo = "";
                                        DataRow dataRow = null;
                                        string prevEnergy = String.Empty;
                                        if (filmConsumptionReportRowInner.ReportNo != prevReportNo)
                                        {
                                            if (filmConsumptionReportRowInner.ReshootNo == 0)
                                            {
                                                dataRow = new DataRow();
                                                dataRow["ReportNo"] = prevReportNo = filmConsumptionReportRowInner.ReportNo; //set prevReportNo for next time
                                                dataRow["ReportDate"] = filmConsumptionReportRowInner.Date;
                                                dataRow["DateOfTest"] = filmConsumptionReportRowInner.DateOfTest;
                                                dataRow["RTNo"] = filmConsumptionReportRowInner.RTNo;
                                                dataRow["FreshCo 60"] = filmConsumptionReportRowInner.AreaInCo;
                                                dataRow["FreshIr 192"] = filmConsumptionReportRowInner.AreaInIr;
                                                rows.Add(dataRow);
                                            }
                                            else
                                            {
                                                RGReport rgReport = rgReportList.Where(p => p.RTNo == filmConsumptionReportRowInner.RTNo &&
                                                    p.ReshootNo == (filmConsumptionReportRowInner.ReshootNo - 1)).FirstOrDefault();
                                                float repairAreaCo = 0;
                                                float reshootAreaCo = 0;
                                                float retakeAreaCo = 0;
                                                float checkshotAreaCo = 0;
                                                float repairAreaIr = 0;
                                                float reshootAreaIr = 0;
                                                float retakeAreaIr = 0;
                                                float checkshotAreaIr = 0;

                                                if (rgReport != null)
                                                {
                                                    foreach (RGReportRow reportRow in rgReport.RGReportRows)
                                                    {
                                                        if (reportRow.RemarkID == 1)
                                                        {
                                                            if (reportRow.EnergyID == 1)
                                                                repairAreaCo += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                            else
                                                                repairAreaIr += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                        }
                                                        else if (reportRow.RemarkID == 3)
                                                        {
                                                            if (reportRow.EnergyID == 1)
                                                                reshootAreaCo += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                            else
                                                                reshootAreaIr += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                        }
                                                        else if (reportRow.RemarkID == 4)
                                                        {
                                                            if (reportRow.EnergyID == 1)
                                                                retakeAreaCo += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                            else
                                                                retakeAreaIr += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                        }
                                                        else if (reportRow.RemarkID == 5)
                                                        {
                                                            if (reportRow.EnergyID == 1)
                                                                checkshotAreaCo += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                            else
                                                                checkshotAreaIr += reportRow.FilmSize.Area * reportRow.FilmCount;
                                                        }
                                                    }
                                                }

                                                dataRow = new DataRow();
                                                dataRow["ReportNo"] = prevReportNo = filmConsumptionReportRowInner.ReportNo; //set prevReportNo for next time
                                                dataRow["ReportDate"] = filmConsumptionReportRowInner.Date;
                                                dataRow["DateOfTest"] = filmConsumptionReportRowInner.DateOfTest;
                                                dataRow["RTNo"] = filmConsumptionReportRowInner.RTNo;
                                                dataRow["FreshCo 60"] = filmConsumptionReportRowInner.AreaInCo;
                                                dataRow["FreshIr 192"] = filmConsumptionReportRowInner.AreaInIr;

                                                foreach (var row in ctx.Energies)
                                                {
                                                    if (row.ID == 1)
                                                    {
                                                        dataRow["Repair" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = repairAreaCo;
                                                        dataRow["Reshoot" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = reshootAreaCo;
                                                        dataRow["Retake" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = retakeAreaCo;
                                                        dataRow["Checkshot" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = checkshotAreaCo;
                                                    }
                                                    else if (row.ID == 2)
                                                    {
                                                        dataRow["Repair" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = repairAreaIr;
                                                        dataRow["Reshoot" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = reshootAreaIr;
                                                        dataRow["Retake" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = retakeAreaIr;
                                                        dataRow["Checkshot" + (filmConsumptionReportRowInner.ReshootNo) + row.Name] = checkshotAreaIr;
                                                        rows.Add(dataRow);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //totals row
                                    var totalRow = new DataRow();
                                    totalRow["FreshCo 60"] = rows.Select(p => (p["FreshCo 60"] as float?) ?? 0).Sum();
                                    totalRow["FreshIr 192"] = rows.Select(p => (p["FreshIr 192"] as float?) ?? 0).Sum();

                                    //Repair columns
                                    for (int i = 0; i < highestNoOfRepair; i++)
                                    {
                                        int rowIndex = 0;
                                        foreach (var row in ctx.Energies)
                                        {
                                            var colName = "Repair" + (i + 1) + row.Name;
                                            totalRow[colName] = rows.Select(p => (p[colName] as float?) ?? 0).Sum();
                                            rowIndex++;
                                        }
                                    }

                                    //Reshoot columns
                                    for (int i = 0; i < highestNoOfRepair; i++)
                                    {
                                        int rowIndex = 0;
                                        foreach (var row in ctx.Energies)
                                        {
                                            var colName = "Reshoot" + (i + 1) + row.Name;
                                            totalRow[colName] = rows.Select(p => (p[colName] as float?) ?? 0).Sum();
                                            rowIndex++;
                                        }
                                    }

                                    //Retake columns
                                    for (int i = 0; i < highestNoOfRepair; i++)
                                    {
                                        int rowIndex = 0;
                                        foreach (var row in ctx.Energies)
                                        {
                                            var colName = "Retake" + (i + 1) + row.Name;
                                            totalRow[colName] = rows.Select(p => (p[colName] as float?) ?? 0).Sum();
                                            rowIndex++;
                                        }
                                    }

                                    //checkshot columns
                                    for (int i = 0; i < highestNoOfRepair; i++)
                                    {
                                        int rowIndex = 0;
                                        foreach (var row in ctx.Energies)
                                        {
                                            var colName = "Checkshot" + (i + 1) + row.Name;
                                            totalRow[colName] = rows.Select(p => (p[colName] as float?) ?? 0).Sum();
                                            rowIndex++;
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

                            }, false);
                        }

                    }

                    if (noOfInnerLoops == 0)
                    {
                        DataRow dataRow = null;
                        foreach (var filmConsumptionReportRowInner in FilmConsumptionReportRowList)
                        {
                            dataRow = new DataRow();
                            dataRow["ReportNo"] = filmConsumptionReportRowInner.ReportNo; //set prevReportNo for next time
                            dataRow["ReportDate"] = filmConsumptionReportRowInner.Date;
                            dataRow["DateOfTest"] = filmConsumptionReportRowInner.DateOfTest;
                            dataRow["RTNo"] = filmConsumptionReportRowInner.RTNo;
                            dataRow["FreshCo 60"] = filmConsumptionReportRowInner.AreaInCo;
                            dataRow["FreshIr 192"] = filmConsumptionReportRowInner.AreaInIr;
                            rows.Add(dataRow);
                        }

                        var totalRow = new DataRow();

                        totalRow["FreshCo 60"] = rows.Select(p => (p["FreshCo 60"] as float?) ?? 0).Sum();
                        totalRow["FreshIr 192"] = rows.Select(p => (p["FreshIr 192"] as float?) ?? 0).Sum();
                        rows.Add(totalRow);

                        var ds = new DataSet("ReportDataSet");
                        ds.Tables.Add(reportTable);

                        reportGrid.DataSource = ds;
                        reportGrid.DataMember = "Report";
                        reportGrid.DataBind();
                        busyIndicator.IsBusy = false;
                    }
                }
                else
                {
                    var ds = new DataSet("ReportDataSet");
                    reportTable = new DataTable("Report");
                    ds.Tables.Add(reportTable);

                    reportGrid.DataSource = ds;
                    reportGrid.DataMember = "Report";
                    reportGrid.DataBind();
                    MessageBox.Show("No records found!!");
                    busyIndicator.IsBusy = false;
                    return;
                }

            }, false);
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
            btnFetch.IsEnabled = ValueChanged();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            btnFetch.IsEnabled = ValueChanged();
        }

        public bool ValueChanged()
        {
            return (!(cmbFoundry.SelectedIndex == -1 || (String.IsNullOrEmpty(txtRTNo.Text) &&
                                                            String.IsNullOrEmpty(txtHeatNo.Text)))
                    || !(cmbFoundry.SelectedIndex == -1 || String.IsNullOrEmpty(txtFPNo.Text) ||
                     cmbCoverage.SelectedIndex == -1));
        }
    }
}
