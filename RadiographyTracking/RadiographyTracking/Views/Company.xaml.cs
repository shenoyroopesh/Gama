using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RadiographyTracking.Web.Services;
using RadiographyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;
using RadiographyTracking.Controls;

namespace RadiographyTracking.Views
{
    public partial class Company : UserControl
    {
        RadiographyContext ctx;
        Web.Models.Company CompanyModel { get; set; }

        public Company()
        {
            InitializeComponent();
        }

        private void companyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {
                ctx = (RadiographyContext)companyDomainDataSource.DomainContext;
                if (ctx.Companies.Count > 0)
                {
                    companyForm.CurrentItem = this.CompanyModel = ctx.Companies.First();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (FileUploader.File != null)
                this.CompanyModel.Logo = FileUploader.File;

            
            if (companyForm.ValidateItem())
            {
                companyForm.CommitEdit();
                ctx.SubmitChanges(OnFormSubmitCompleted, null);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (companyForm.CommitEdit())
                ctx.RejectChanges();
        }

        /// <summary>
        /// This is used to handle the domaincontext submit operation result, and check whether there are any errors. 
        /// If not, user is show success message
        /// </summary>
        /// <param name="so"></param>
        public void OnFormSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message), "Error", MessageBoxButton.OK);
                so.MarkErrorAsHandled();
            }
            else
            {
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButton.OK);
            }
        }

        private void companyForm_AutoGeneratingField(object sender, DataFormAutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName == "Logo")
            {
                FileUpload fileUploader = new FileUpload();
                e.Field.ReplaceTextBox(fileUploader, FileUpload.UploadedFileProperty);
            }
        }
    }
}
