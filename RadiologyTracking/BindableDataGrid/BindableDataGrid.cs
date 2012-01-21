using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Reflection;
using BindableDataGrid.Data;
using System.Windows.Data;
using System.Windows;
using System.Text;
using System.Windows.Markup;
using System.Collections;

namespace BindableDataGrid
{
    /// <summary>
    /// New DataGrid that enables the use of DataSource property and DataBind method
    /// </summary>
    public class BindableDataGrid : DataGrid
    {
        #region "Properties"
        
        /// <summary>
        /// DataSource of the DataGrid
        /// </summary>
        public IDataSource DataSource { get; set; }

        /// <summary>
        /// Name of the member in the DataSource
        /// </summary>
        public string DataMember { get; set; }

        /// <summary>
        /// Gets or sets the data item corresponding to the selected row
        /// </summary>
        public new DataRow SelectedItem {
            get
            {
                return this.GetDataRowFromObject(base.SelectedItem);
            }
            set
            {
                if (value != null) // If there's something selected then we rebind data
                {
                    int index = -1;
                    foreach (object o in this.ItemsSource)
                    {
                        index++;
                        DataRow dr = this.GetDataRowFromObject(o);
                        if (value.Equals(dr))
                        {
                            base.SelectedIndex = index;
                            break; // Exit foreach
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list that contains the data items corresponding to the selected rows.
        /// </summary>
        public new DataRowCollection SelectedItems
        {
            get
            {
                // Always return a collection (empty or not)
                DataRowCollection drcol = new DataRowCollection();
                if (base.SelectedItems.Count != 0)
                {
                    foreach (object rows in base.SelectedItems)
                    {
                        drcol.Add(this.GetDataRowFromObject(rows));
                    }
                }
                return drcol;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Default constructor of the class. Will take care of formatting columns upon creation
        /// </summary>
        public BindableDataGrid()
        {
            // If autogeneration of columns is ON, will need to do some custom work as well
            this.AutoGeneratingColumn += new EventHandler<DataGridAutoGeneratingColumnEventArgs>(BindableDataGrid_AutoGeneratingColumn);
        }

        /// <summary>
        /// Parses the DataSource property and creates the necessary columns
        /// </summary>
        public void DataBind()
        {
            if (this.DataSource == null)
            {
                throw new NullReferenceException("BindableDataGrid: DataSource is null");
            }
            if (!(this.DataSource is IDataSource))
            {
                throw new NotSupportedException(String.Format("BindableDataGrid: DataSource type '{0}' not supported", this.DataSource.GetType().Name));
            }

            List<object> values = null;
            DataColumnCollection dcol = null;

            switch (this.DataSource.GetType().Name)
            {
                case "DataTable":
                    dcol = ((DataTable)this.DataSource).Columns;
                    values = this.GetValuesFromDataTable((DataTable)this.DataSource);
                    break;
                case "DataSet":
                    if (String.IsNullOrEmpty(this.DataMember))
                    {
                        throw new NullReferenceException("BindableDataGrid: DataMember not specified");
                    }
                    DataTable tempDT = ((DataSet)this.DataSource).Tables[this.DataMember];
                    if (tempDT == null)
                    {
                        throw new NullReferenceException(String.Format("BindableDataGrid: DataMember '{0}' doesn't exist", this.DataMember));
                    }
                    dcol = tempDT.Columns;
                    values = this.GetValuesFromDataTable(tempDT);
                    break;
                default:
                    throw new NotImplementedException(String.Format("BindableDataGrid: DataSource type '{0}' not yet implemented", this.DataSource.GetType().Name));
            }
            // New to manually create the columns (handles Image types, etc...)
            if (!this.AutoGenerateColumns)
            {
                this.CreateColumns(dcol);
            }
            // Assign the list of objects to the ItemsSource property of the grid
            this.ItemsSource = values;
        }

        /// <summary>
        /// Creates the columns when AutoGenerateColumns=false (takes care of Images, etc...)
        /// </summary>
        /// <param name="dcol">DataColumn collection</param>
        private void CreateColumns(DataColumnCollection dcol)
        {
            this.Columns.Clear();
            foreach (DataColumn dc in dcol)
            {
                DataGridColumn dgc = null;
                switch (dc.DataType.Name)
                {
                    case "Boolean":
                        DataGridCheckBoxColumn checkBoxColumn = new DataGridCheckBoxColumn();
                        checkBoxColumn.Binding = new Binding(dc.ColumnName);
                        dgc = checkBoxColumn;
                        break;
                    case "Image":
                        DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                        // Build template in memory
                        StringBuilder CellTemp = new StringBuilder();
                        CellTemp.Append("<DataTemplate ");
                        CellTemp.Append("xmlns='http://schemas.microsoft.com/winfx/");
                        CellTemp.Append("2006/xaml/presentation' ");
                        CellTemp.Append("xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' ");
                        CellTemp.Append("xmlns:local='clr-namespace:BindableDataGrid;assembly=BindableDataGrid'>");
                        CellTemp.Append("<Image Source='{Binding " + dc.ColumnName + ".Source }' ></Image>");
                        CellTemp.Append("</DataTemplate>");
                        templateColumn.CellTemplate = (DataTemplate)XamlReader.Load(CellTemp.ToString());
                        // Set editing template the same as regular one (will not be editable)
                        templateColumn.CellEditingTemplate = (DataTemplate)XamlReader.Load(CellTemp.ToString());
                        dgc = templateColumn;
                        break;
                    //case "String":
                    //case "Int32":
                    //case "DateTime":
                    default: // Treat everything else as a string
                        DataGridTextColumn textColumn = new DataGridTextColumn();
                        textColumn.Binding = new Binding(dc.ColumnName);
                        dgc = textColumn;
                        break;
                }
                if (dgc != null)
                {
                    dgc.Header = dc.Caption;
                    dgc.IsReadOnly = dc.ReadOnly;
                    dgc.CanUserResize = dc.AllowResize;
                    dgc.CanUserSort = dc.AllowSort;
                    dgc.CanUserReorder = dc.AllowReorder;
                    this.Columns.Add(dgc);
                }
            }
        }

        /// <summary>
        /// Given a DataRow object (from reflection) get a "real" DataRow object to be used with the
        /// SelectedItem and SelectedItems properties
        /// </summary>
        /// <param name="o">Object coming from the base DataGrid</param>
        /// <returns>DataRow</returns>
        private DataRow GetDataRowFromObject(object o)
        {
            DataRow dr = null;
            if (o != null)
            {
                dr = new DataRow();
                foreach (PropertyInfo pi in o.GetType().GetProperties())
                {
                    dr.Items.Add(pi.Name, pi.GetValue(o, null));
                }
            }
            return dr;
        }

        /// <summary>
        /// Process the DataTable and return a list of dynamic objects
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <returns>List of objects representing the rows in the DataTable</returns>
        private List<object> GetValuesFromDataTable(DataTable dt)
        {
            List<object> values = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                // Create an object of the dynamic class that represents a row
                Assembly tmp = dr.EmitAssembly();
                object c = tmp.CreateInstance("DataRowObject");
                Type myObject = tmp.GetType("DataRowObject");
                // Add the values to the properties
                foreach (string key in dr.Items.Keys)
                {
                    PropertyInfo pi = myObject.GetProperty(key);
                    pi.SetValue(c, dr.Items[key], null);
                }
                // Add the object to the generic list
                values.Add(c);
            }
            return values;
        }

        /// <summary>
        /// Changes the properties of the columns upon generation, when AutoGenerateColumns is set to true
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event Args</param>
        private void BindableDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataTable dt = null;
            switch (this.DataSource.GetType().Name)
            {
                case "DataTable":
                    dt = ((DataTable)this.DataSource);
                    break;
                case "DataSet":
                    if (String.IsNullOrEmpty(this.DataMember))
                    {
                        throw new NullReferenceException("BindableDataGrid: DataMember not specified");
                    }
                    dt = ((DataSet)this.DataSource).Tables[this.DataMember];
                    if (dt == null)
                    {
                        throw new NullReferenceException(String.Format("BindableDataGrid: DataMember '{0}' doesn't exist", this.DataMember));
                    }
                    dt = ((DataSet)this.DataSource).Tables[this.DataMember];
                    break;
                default:
                    throw new NotSupportedException(String.Format("BindableDataGrid: DataSource type '{0}' not supported", this.DataSource.GetType().Name));
            }
            // Assign the properties found in the DataColumnCollection of the table
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName == e.Column.Header.ToString())
                {
                    e.Column.Header = dc.Caption;
                    e.Column.IsReadOnly = dc.ReadOnly;
                    e.Column.CanUserResize = dc.AllowResize;
                    e.Column.CanUserSort = dc.AllowSort;
                    e.Column.CanUserReorder = dc.AllowReorder;
                    break;
                }
            }            
        }

        #endregion
    }
}