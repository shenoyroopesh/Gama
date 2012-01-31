using System;
using System.Collections.ObjectModel;

namespace BindableDataGrid.Data
{
    /// <summary>
    /// Collection of tables
    /// </summary>
    public class DataTableCollection : ObservableCollection<DataTable>
    {
        #region "Properties"

        /// <summary>
        /// Indexer to access the tables based on table name
        /// </summary>
        /// <param name="key">Name of the table</param>
        /// <returns>DataTable</returns>
        public DataTable this[string key]
        {
            get
            {
                DataTable ret = null;
                foreach (DataTable dt in this)
                {
                    if (dt.Name == key)
                    {
                        ret = dt;
                        break; // Exit foreach
                    }
                }
                return ret;
            }
        }

        #endregion

        #region "Methods"

        /// <summary>
        /// Adds a new table to the collection checking for duplicates in the name
        /// </summary>
        /// <param name="dt">New DataTable to add</param>
        public new void Add(DataTable dt)
        {
            foreach (DataTable curTable in this)
            {
                if (dt.Name == curTable.Name)
                {
                    throw new Exception(String.Format("DataTableCollection: Table with name '{0}' already exists", dt.Name));
                }
            }
            base.Add(dt);
        }

        #endregion
    }
}