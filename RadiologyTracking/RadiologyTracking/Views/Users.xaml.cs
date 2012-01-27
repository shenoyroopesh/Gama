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
using RadiologyTracking.Web.Services;

namespace RadiologyTracking.Views
{
    public partial class Users : UserControl
    {
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private RadiologyContext radiologyContext = new RadiologyContext();

        public Users()
            : base()
        {
            InitializeComponent();
            loadUsers();
        }

        private void loadUsers()
        {
            busyIndicator.IsBusy = true;
            userRegistrationContext.GetUsers(OnLoadUsers, null);
            radiologyContext.Load(radiologyContext.GetFoundriesQuery()).Completed += Foundries_Loaded;
            radiologyContext.GetRoles(Roles_Loaded, null);
        }

        public IEnumerable<String> Roles { get; set; }

        public IEnumerable<String> Foundries { get; set; }

        void Foundries_Loaded(object sender, EventArgs e)
        {
            Foundries = radiologyContext.Foundries.Select(p => p.FoundryName);
        }

        void Roles_Loaded(InvokeOperation<IEnumerable<String>> op)
        {
            Roles = op.Value;
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
            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            var registrationData = (RegistrationData)row.DataContext;
            registrationData.isEditing = true;
            OpenRegistrationWindow(registrationData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOperation(object sender, RoutedEventArgs e)
        {
            OpenRegistrationWindow(new RegistrationData() { isEditing = false });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regData"></param>
        private void OpenRegistrationWindow(RegistrationData regData)
        {
            RegistrationFormWindow registerWindow = new RegistrationFormWindow();
            var form = registerWindow.RegistrationForm;
            form.DataContext = regData;
            form.IsEditing = regData.isEditing;
            form.Foundries = this.Foundries;
            form.Roles = this.Roles;
            registerWindow.Closed += new EventHandler(registerWindow_Closed);
            registerWindow.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void registerWindow_Closed(object sender, EventArgs e)
        {
            //reload users to get latest changes
            loadUsers();
        }

        /// <summary>
        /// This handles the delete operation for this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DeleteOperation(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            var user = row.DataContext as RegistrationData;

            //safety check to prevent automatic lockout.
            if (user.UserName == "admin")
            {
                MessageBox.Show("Cannot delete main admin user");
                return;
            }
            userRegistrationContext.DeleteUser(user.UserName, DeleteCompleted, null);
        }

        /// <summary>
        /// After delete completes loads the users again
        /// </summary>
        /// <param name="op"></param>
        public void DeleteCompleted(InvokeOperation op)
        {
            loadUsers();
        }
    }
}
