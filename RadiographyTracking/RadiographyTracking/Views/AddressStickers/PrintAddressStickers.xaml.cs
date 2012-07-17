namespace RadiographyTracking.AddressStickers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using RadiographyTracking.Web.Services;
    using System.Windows.Browser;

    /// <summary>
    /// <see cref="ChildWindow"/> class that controls the registration process.
    /// </summary>
    public partial class PrintAddressStickers : ChildWindow
    {
        InvokeOperation<IEnumerable<string>> operation;

        /// <summary>
        /// Creates a new <see cref="LoginRegistrationWindow"/> instance.
        /// </summary>
        public PrintAddressStickers()
        {
            InitializeComponent();
            var ctx = new RadiographyContext();
            operation = ctx.GetAddressStickerTemplates();
            operation.Completed += new EventHandler(operation_Completed);
        }

        void operation_Completed(object sender, EventArgs e)
        {
            cmbAddressStickerTemplates.ItemsSource = operation.Value;
        }

        public string ReportNo { get; set; }

        /// <summary>
        /// Ensures the visual state and focus are correct when the window is opened.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
        }


        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chkPrintAddressStickers.IsChecked)
            {
                if (cmbAddressStickerTemplates.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a report template");
                    return;
                }

                int cellNo;

                if(!Int32.TryParse(txtCellNo.Text, out cellNo))
                {
                    MessageBox.Show("Please enter a number in the cell no");
                    return;
                }

                if(cellNo < 1 || cellNo > 12)
                {
                    MessageBox.Show("Please enter a number between 1 and 12 in the cell no");
                    return;
                }

                //Get the root path for the XAP
                string src = Application.Current.Host.Source.ToString();

                //Get the application root, where 'ClientBin' is the known dir where the XAP is
                string appRoot = src.Substring(0, src.IndexOf("ClientBin"));

                Uri reportURI = new Uri(string.Format(appRoot + "AddressStickerReportGenerate.aspx?TEMPLATE_NAME={0}&REPORT_NUMBER={1}&CELL_NO={2}",
                                                        cmbAddressStickerTemplates.SelectedValue, 
                                                        this.ReportNo, 
                                                        cellNo),
                                        UriKind.Absolute);

                HtmlPage.Window.Navigate(reportURI, "_blank");
            }

            this.Close();
        }

    }
}