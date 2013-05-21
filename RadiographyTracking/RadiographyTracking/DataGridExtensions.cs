using System;
using System.IO.IsolatedStorage;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

public static class DataGridExtensions
{
    public const String FILE_FILTER = "CSV Files (*.csv)|*.csv|XML (*.xml)|*.xml|All files (*.*)|*.*";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="dg"></param>
    /// <param name="author">Author of the document</param>
    /// <param name="companyName">Company name</param>
    /// <param name="mergeCells">Cells to be merged. This is to be given in the following notation, separated by commas: 
    /// rowIndex-ColumnIndex-mergeAcross. For eg. for cell no 6 in row 1 to merge across 4 other cells send "1-6-4"</param>
    /// <param name="boldHeaderRows">How many rows from top to be bold</param>
    public static void Export(this DataGrid dg, String author, String companyName, String mergeCells, int boldHeaderRows)
    {
        ExportDataGrid(dg, author, companyName, mergeCells, boldHeaderRows);
    }

    public static void Export(this DataGrid dg, String author, String companyName, String mergeCells, int boldHeaderRows, string fileName)
    {
        ExportDataGrid(dg, author, companyName, mergeCells, boldHeaderRows,fileName);
    }

    public static void ExportDataGrid(DataGrid dGrid, String author, String companyName, String mergeCells, int boldHeaderRows)
    {
        SaveFileDialog objSFD = new SaveFileDialog() { DefaultExt = "xml", Filter = FILE_FILTER, FilterIndex = 2 };

        if (objSFD.ShowDialog() == true)
        {
            string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
            Stream outputStream = objSFD.OpenFile();

            ExportToExcel(dGrid, author, companyName, mergeCells, boldHeaderRows, strFormat, outputStream);
        }
    }

    public static void ExportDataGrid(DataGrid dGrid, String author, String companyName, String mergeCells, int boldHeaderRows, string fileName)
    {
        if (Application.Current.HasElevatedPermissions)
        {

            var filePath ="C:" + Path.DirectorySeparatorChar +"Nithesh"+Path.DirectorySeparatorChar +fileName + ".XML";

            File.Create(filePath);
            Stream outputStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);


            ExportToExcel(dGrid, author, companyName, mergeCells, boldHeaderRows, "XML", outputStream);
        }
        else
        {
            SaveFileDialog objSFD = new SaveFileDialog() { DefaultExt = "xml", Filter = FILE_FILTER, FilterIndex = 2, DefaultFileName = fileName };

            if (objSFD.ShowDialog() == true)
            {
                string strFormat = objSFD.SafeFileName.Substring(objSFD.SafeFileName.IndexOf('.') + 1).ToUpper();
                Stream outputStream = objSFD.OpenFile();

                ExportToExcel(dGrid, author, companyName, mergeCells, boldHeaderRows, strFormat, outputStream);
            }
        }

     

    }

    public static void ExportToExcel(DataGrid dGrid, string author, string companyName, string mergeCells,
                                      int boldHeaderRows, string strFormat, Stream outputStream)
    {
        List<List<String>> mergeCellsList = new List<List<string>>();
        if (!String.IsNullOrEmpty(mergeCells))
        {
            List<String> temp = mergeCells.Split(',').ToList();
            temp.ForEach(p => mergeCellsList.Add(p.Split('-').ToList()));
        }

        StringBuilder strBuilder = new StringBuilder();
        if (dGrid.ItemsSource == null) return;
        List<string> lstFields = new List<string>();
        if (dGrid.HeadersVisibility == DataGridHeadersVisibility.Column ||
            dGrid.HeadersVisibility == DataGridHeadersVisibility.All)
        {
            foreach (DataGridColumn dgcol in dGrid.Columns)
                lstFields.Add(FormatField(dgcol.Header.ToString(), strFormat, true));
            BuildStringOfRow(strBuilder, lstFields, strFormat);
        }

        int rowCount = 0;

        foreach (object data in dGrid.ItemsSource)
        {
            lstFields.Clear();
            for (int colCount = 0; colCount < dGrid.Columns.Count; colCount++)
            //foreach (DataGridColumn col in dGrid.Columns)
            {
                DataGridColumn col = dGrid.Columns[colCount];
                string strValue = "";
                Binding objBinding = null;
                if (col is DataGridBoundColumn)
                    objBinding = (col as DataGridBoundColumn).Binding;
                if (col is DataGridTemplateColumn)
                {
                    //This is a template column... let us see the underlying dependency object
                    DependencyObject objDO = (col as DataGridTemplateColumn).CellTemplate.LoadContent();
                    FrameworkElement oFE = (FrameworkElement)objDO;
                    FieldInfo oFI = oFE.GetType().GetField("TextProperty");
                    if (oFI != null)
                    {
                        var value = oFI.GetValue(null);
                        if (value != null)
                        {
                            var depProp = (DependencyProperty)value;
                            if (oFE.GetBindingExpression(depProp) != null)
                                objBinding = oFE.GetBindingExpression(depProp).ParentBinding;
                        }
                    }
                }
                if (objBinding != null)
                {
                    if (objBinding.Path.Path != "")
                    {
                        PropertyInfo pi = data.GetType().GetProperty(objBinding.Path.Path);
                        if (pi != null) strValue = pi.GetValue(data, null).ToString();
                    }
                    if (objBinding.Converter != null)
                    {
                        if (strValue != "")
                            strValue =
                                objBinding.Converter.Convert(strValue, typeof(string), objBinding.ConverterParameter,
                                                             objBinding.ConverterCulture).ToString();
                    }
                }

                // In mergeCellsList for each element, 0 has row num, 1 has column num, and 4 has number of cells to merge with this cell

                var merge =
                    mergeCellsList.FirstOrDefault(
                        p => Convert.ToInt32(p[0]) == rowCount && Convert.ToInt32(p[1]) == colCount);
                var bold = (rowCount < boldHeaderRows); //only header rows need to be bold

                lstFields.Add(FormatField(strValue, strFormat, bold, merge == null ? null : merge[2]));
                if (merge != null) colCount += Convert.ToInt32(merge[2]); //don't process the merged cells
            }
            BuildStringOfRow(strBuilder, lstFields, strFormat);
            rowCount++;
        }

        StreamWriter sw = new StreamWriter(outputStream);
        if (strFormat == "XML" || strFormat == "XLSX")
        {
            //Let us write the headers for the Excel XML
            WriteExcelHeader(sw, author, companyName);
        }
        sw.Write(strBuilder.ToString());
        if (strFormat == "XML" || strFormat == "XLSX")
        {
            CloseTags(sw);
        }
        sw.Close();
    }

    private static void CloseTags(StreamWriter sw)
    {
        sw.WriteLine("</Table>");
        sw.WriteLine("</Worksheet>");
        sw.WriteLine("</Workbook>");
    }

    private static void WriteExcelHeader(StreamWriter sw, String author, String companyName)
    {
        sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
        sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" " +
                               "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">"); //check if this alias is really needed
        sw.WriteLine("<DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">");
        sw.WriteLine("<Author>" + author + "</Author>");
        sw.WriteLine("<Created>" + DateTime.Now.ToLocalTime().ToLongDateString() + "</Created>");
        sw.WriteLine("<LastSaved>" + DateTime.Now.ToLocalTime().ToLongDateString() + "</LastSaved>");
        sw.WriteLine("<Company>" + companyName + "</Company>");
        sw.WriteLine("<Version>12.00</Version>");
        sw.WriteLine("</DocumentProperties>");
        sw.WriteLine("<Styles>");
        sw.WriteLine("  <Style ss:ID=\"sCenter\" ss:Name=\"Normal\">");
        sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
        sw.WriteLine("   <Borders>");
        sw.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("   </Borders>");
        sw.WriteLine("  </Style>");
        sw.WriteLine("  <Style ss:ID=\"sCenterBold\">");
        sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
        sw.WriteLine("   <Font ss:Family=\"Verdana\" ss:Bold=\"1\"/>");
        sw.WriteLine("   <Borders>");
        sw.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("   </Borders>");
        sw.WriteLine("  </Style>");
        sw.WriteLine("  <Style ss:ID=\"sCenterRed\">");
        sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
        sw.WriteLine("   <Interior ss:Color=\"Red\" ss:Pattern=\"Solid\"/>");
        sw.WriteLine("   <Font ss:Family=\"Verdana\"/>");
        sw.WriteLine("   <Borders>");
        sw.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("   </Borders>");
        sw.WriteLine("  </Style>");
        sw.WriteLine("  <Style ss:ID=\"sCenterBlue\">");
        sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
        sw.WriteLine("   <Interior ss:Color=\"DeepSkyBlue\" ss:Pattern=\"Solid\"/>");
        sw.WriteLine("   <Font ss:Family=\"Verdana\" />");
        sw.WriteLine("   <Borders>");
        sw.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("   </Borders>");
        sw.WriteLine("  </Style>");
        sw.WriteLine("  <Style ss:ID=\"sCenterYellow\">");
        sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/>");
        sw.WriteLine("   <Interior ss:Color=\"Yellow\" ss:Pattern=\"Solid\"/>");
        sw.WriteLine("   <Font ss:Family=\"Verdana\"/>");
        sw.WriteLine("   <Borders>");
        sw.WriteLine("      <Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("      <Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\"/>");
        sw.WriteLine("   </Borders>");
        sw.WriteLine("  </Style>");
        sw.WriteLine("</Styles>");
        sw.WriteLine("<Worksheet ss:Name=\"Silverlight Export\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
        sw.WriteLine("<Table>");
    }

    private static void BuildStringOfRow(StringBuilder strBuilder, List<string> lstFields, string strFormat)
    {
        switch (strFormat)
        {
            case "XML":
            case "XLSX":
                strBuilder.AppendLine("<Row>");
                strBuilder.AppendLine(String.Join("\r\n", lstFields.ToArray()));
                strBuilder.AppendLine("</Row>");
                break;
            case "CSV":
                strBuilder.AppendLine(String.Join(",", lstFields.ToArray()));
                break;
        }
    }
    private static string FormatField(string data, string format, bool bold = false, string merge = null)
    {
        string style = bold ? "sCenterBold" : "sCenter";
       
        switch (format)
        {
            case "XML":
            case "XLSX":
                string outputFieldFormat;
                if (merge == null)
                {
                    String type = "String";
                    decimal number = 0;
                    //check if data is number
                    if (Regex.IsMatch(data, @"^[\-0-9]\d*(\.\d+)?$"))
                        if (decimal.TryParse(data, out number))
                            type = "Number";

                    data = FormatFieldColour(data, ref style);

                    outputFieldFormat = String.Format("<Cell ss:StyleID=\"" + style + "\"" + "><Data ss:Type=\"{1}\">{0}</Data>" +
                                                 "</Cell>", data, type);
                }
                else
                {
                    outputFieldFormat = String.Format("<Cell ss:MergeAcross=\"" + merge +
                                                 "\" ss:StyleID=\"" + style + "\"><Data ss:Type=\"String\" >{0}</Data>" +
                                                 "</Cell>", data);
                }
                return outputFieldFormat;
            //merge is applicable only for xml format not for csv
            case "CSV":
                return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
        }
        return data;
    }

    private static string FormatFieldColour(string data, ref string style)
    {
        if (data.EndsWith("^^^"))
        {
            style = "sCenterBlue";
            data = data.Replace("^^^", "");
        }
        else if (data.EndsWith("^^"))
        {
            style = "sCenterYellow";
            data = data.Replace("^^", "");
        }
        else if (data.EndsWith("^"))
        {
            style = "sCenterRed";
            data = data.Replace("^", "");
        }
        else
            style = style;
        return data;
    }
}