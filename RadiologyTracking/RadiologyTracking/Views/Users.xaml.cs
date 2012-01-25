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
using RadiologyTracking.Web;
using System.ServiceModel.DomainServices.Client;
using RadiologyTracking.LoginUI;

namespace RadiologyTracking.Views
{
    public partial class Users : UserControl
    {
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        public Users()
        {
            InitializeComponent();
            loadUsers();
        }

        private void loadUsers()
        {
            busyIndicator.IsBusy = true;
            userRegistrationContext.GetUsers(OnLoadUsers, null);
        }

        private void OnLoadUsers(InvokeOperation<IEnumerable<RegistrationData>> op)
        {
            if (op.HasError)
            {
                MessageBox.Show(op.Error.Message);
            }
            else
            {
                DataContext = op.Value;
            }
            busyIndicator.IsBusy = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditOperation(object sender, RoutedEventArgs e)
        {

        }

        private void AddOperation(object sender, RoutedEventArgs e)
        {
            RegistrationForm form = new RegistrationForm();
        }
    }
}
