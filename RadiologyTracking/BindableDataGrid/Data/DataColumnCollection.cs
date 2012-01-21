using System;
using System.Collections.ObjectModel;

namespace BindableDataGrid.Data
{
    /// <summary>
    /// Collection of columns in a DataTable
    /// </summary>
    public class DataColumnCollection : ObservableCollection<DataColumn>
    {
        #region "Properties"

        /// <summary>
        /// Indexer to access the columns based on column name
        /// </summary>
        /// <param name="key">Name of the column</param>
        /// <returns>DataColumns</returns>
        public DataColumn this[string key]
        {
            get
            {
                DataColumn ret = null;
                foreach (DataColumn dc in this)
                {
                    if (dc.ColumnName == key)
                    {
                        ret = dc;
                        break; // Exit foreach
                    }
                }
                return ret;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Adds a new column to the collection checking for duplicates in the name
        /// </summary>
        /// <param name="dc">New column to add</param>
        public new void Add(DataColumn dc)
        {
            foreach (DataColumn curColumn in this)
            {
                if (dc.ColumnName == curColumn.ColumnName)
                {
                    throw new Exception(String.Format("DataColumnCollection: Column with name '{0}' already exists", dc.ColumnName));
                }
            }
            base.Add(dc);
        }

        #endregion
    }
}