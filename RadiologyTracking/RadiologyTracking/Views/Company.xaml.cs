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
using RadiologyTracking.Web.Services;
using RadiologyTracking.Web.Models;
using System.ServiceModel.DomainServices.Client;

namespace RadiologyTracking.Views
{
    public partial class Company : UserControl
    {
        RadiologyContext ctx;


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
                ctx = (RadiologyContext)companyDomainDataSource.DomainContext;
                if (ctx.Companies.Count > 0)
                {
                    companyForm.CurrentItem = ctx.Companies.First();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(companyForm.ValidateItem())
                if (companyForm.CommitEdit())
                    ctx.SubmitChanges(OnFormSubmitCompleted, null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(companyForm.CommitEdit())
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
    }
}
