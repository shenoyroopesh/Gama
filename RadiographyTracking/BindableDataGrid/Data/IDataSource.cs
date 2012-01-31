namespace BindableDataGrid.Data
{
    /// <summary>
    /// Simple interface to define what objects can be used as DataSource for the grid
    /// </summary>
    public interface IDataSource
    {
        #region "Properties"

        /// <summary>
        /// Name of the DataSource
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}