namespace BindableDataGrid.Data
{
    /// <summary>
    /// Represents a DataTable
    /// </summary>
    public class DataTable : IDataSource
    {
        #region "Properties"

        /// <summary>
        /// Name of the DataTable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of columns
        /// </summary>
        public DataColumnCollection Columns { get; set; }

        /// <summary>
        /// Collection of rows
        /// </summary>
        public DataRowCollection Rows { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Constructor of the DataTable with a name
        /// </summary>
        /// <param name="name">Name of the DataTable</param>
        public DataTable(string name)
        {
            this.Name = name;
            this.Columns = new DataColumnCollection();
            this.Rows = new DataRowCollection();
        }

        #endregion
    }
}