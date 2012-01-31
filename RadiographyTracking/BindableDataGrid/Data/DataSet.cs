using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace BindableDataGrid.Data
{
    public class DataSet : IDataSource
    {
        #region "Properties"

        /// <summary>
        /// Name of the DataSet
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List of DataTables
        /// </summary>
        public DataTableCollection Tables { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="name"></param>
        public DataSet(string name)
        {
            this.Name = name;
            this.Tables = new DataTableCollection();
        }

        #endregion
    }
}