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
                    ctx.SubmitChanges(BaseCRUDView.OnFormSubmitCompleted, null);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if(companyForm.CommitEdit())
                ctx.RejectChanges();
        }
    }
}
