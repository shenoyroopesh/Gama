using System;

namespace BindableDataGrid.Data
{
    /// <summary>
    /// Represents a column of data in a DataTable
    /// </summary>
    public class DataColumn
    {
        #region "Properties"

        /// <summary>
        /// Name of the column
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Data type of the column
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Caption to be used in the column header
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Allow the user to resize the column
        /// </summary>
        public bool AllowResize { get; set; }

        /// <summary>
        /// Allow the user to sort this column
        /// </summary>
        public bool AllowSort { get; set; }

        /// <summary>
        /// Allow the user to reorder this column
        /// </summary>
        public bool AllowReorder { get; set; }

        /// <summary>
        /// Column is read only
        /// </summary>
        public bool ReadOnly { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Constructor with a column name.
        /// The caption (header) will be the same.
        /// Defaults to "String" type
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        public DataColumn(string columnName)
        {
            this.ColumnName = columnName;
            // Default values
            this.Caption = columnName; // By default it will be the same unless we change it
            this.AllowResize = true;
            this.AllowSort = true;
            this.AllowReorder = true;
            this.ReadOnly = false;
            this.DataType = typeof(String);
        }

        /// <summary>
        /// Constructor with a column name and a type.
        /// The caption (header) will be the same as the column name.
        /// </summary>
        /// <param name="columnName">Name of the column</param>
        /// <param name="columnType">Type of the column</param>
        public DataColumn(string columnName, Type columnType) : this(columnName)
        {
            this.DataType = columnType;
        }

        #endregion
    }
}