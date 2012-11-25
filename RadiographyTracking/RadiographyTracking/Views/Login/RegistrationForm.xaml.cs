namespace RadiographyTracking.LoginUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using RadiographyTracking.Web;
    using RadiographyTracking.Web.Models;
    using RadiographyTracking.Web.Services;
    using RadiographyTracking.Views;

    /// <summary>
    /// Form that presents the <see cref="RegistrationData"/> and performs the registration process.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private RegistrationFormWindow parentWindow;
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private RadiographyContext radiologyContext = new RadiographyContext();
        private TextBox userNameTextBox;
        private ComboBox customerCombobox;
        private ComboBox roleCombobox;
        private ComboBox foundryCombobox;
        /// <summary>
        /// Creates a new <see cref="RegistrationForm"/> instance.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Sets the parent window for the current <see cref="RegistrationForm"/>.
        /// </summary>
        /// <param name="window">The window to use as the parent.</param>
        public void SetParentWindow(RegistrationFormWindow window)
        {
            this.parentWindow = window;
        }


        public IEnumerable<String> Roles { get; set; }

        public IEnumerable<String> Foundries { get; set; }

        public IEnumerable<String> CustomerOfFoundry { get; set; }

        public bool IsEditing { get; set; }

        /// <summary>
        /// Wire up the Password and PasswordConfirmation accessors as the fields get generated.
        /// Also bind the Question field to a ComboBox full of security questions, and handle the LostFocus event for the UserName TextBox.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // Put all the fields in adding mode
            e.Field.Mode = DataFieldMode.AddNew;

            var registrationData = DataContext as RegistrationData;

            if (e.PropertyName == "UserName")
            {
                this.userNameTextBox = (TextBox)e.Field.Content;
                this.userNameTextBox.LostFocus += this.UserNameLostFocus;
            }
            else if (e.PropertyName == "Password")
            {
                PasswordBox passwordBox = new PasswordBox();
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty);
                registrationData.PasswordAccessor = () => passwordBox.Password;
            }
            else if (e.PropertyName == "PasswordConfirmation")
            {
                PasswordBox passwordConfirmationBox = new PasswordBox();
                e.Field.ReplaceTextBox(passwordConfirmationBox, PasswordBox.PasswordProperty);
                registrationData.PasswordConfirmationAccessor = () => passwordConfirmationBox.Password;
            }
            else if (e.PropertyName == "Foundry")
            {
                this.foundryCombobox = new ComboBox();
                foundryCombobox.ItemsSource = Foundries;
                e.Field.ReplaceTextBox(foundryCombobox, ComboBox.SelectedItemProperty);
                foundryCombobox.SelectionChanged += this.foundryComboboxSelectionChanged;
            }
            else if (e.PropertyName == "Role")
            {
                roleCombobox = new ComboBox();
                roleCombobox.ItemsSource = Roles;
                e.Field.ReplaceTextBox(roleCombobox, ComboBox.SelectedItemProperty);
                roleCombobox.SelectionChanged += this.roleComboboxSelectionChanged;
            }

            else if (e.PropertyName == "CustomerCompany")
            {
                this.customerCombobox = new ComboBox();
                e.Field.ReplaceTextBox(customerCombobox, ComboBox.SelectedItemProperty);
            }
        }

        /// <summary>
        /// The callback for when the UserName TextBox loses focus.
        /// Call into the registration data to allow logic to be processed, possibly setting the FriendlyName field.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            (DataContext as RegistrationData).UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Submit the new registration.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // We need to force validation since we are not using the standard OK button from the DataForm.
            // Without ensuring the form is valid, we would get an exception invoking the operation if the entity is invalid.
            if (this.ValidateItem())
            {
                var registrationData = DataContext as RegistrationData;
                if (IsEditing)
                {
                    registrationData.CurrentOperation = this.userRegistrationContext.EditUser(
                                                                        registrationData,
                                                                        registrationData.Password,
                                                                        RegistrationOperation_Completed, null);
                }
                else
                {
                    registrationData.CurrentOperation = this.userRegistrationContext.CreateUser(
                                                                        registrationData,
                                                                        registrationData.Password,
                                                                        RegistrationOperation_Completed, null);
                }
            }
        }

        private bool ValidateItem()
        {
            var data = DataContext as RegistrationData;
            if (data.isEditing && data.Password == "")
            {
                //check is mainly in password, which is not needed now
                return true;
            }
            else
            {
                return this.registerForm.ValidateItem();
            }
        }



        /// <summary>
        /// Completion handler for the registration operation. 
        /// If there was an error, an <see cref="ErrorWindow"/> is displayed to the user.
        /// Otherwise, this triggers a login operation that will automatically log in the just registered user.
        /// </summary>
        private void RegistrationOperation_Completed(InvokeOperation operation)
        {
            if (!operation.IsCanceled)
            {
                if (operation.HasError)
                {
                    ErrorWindow.CreateNew(operation.Error);
                    operation.MarkErrorAsHandled();
                }
                else
                {
                    this.parentWindow.Close();
                }
            }
        }

        /// <summary>
        /// If a registration or login operation is in progress and is cancellable, cancel it.
        /// Otherwise, close the window.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            var registrationData = DataContext as RegistrationData;

            if (registrationData.CurrentOperation != null && registrationData.CurrentOperation.CanCancel)
            {
                registrationData.CurrentOperation.Cancel();
            }
            else if (this.parentWindow != null)
            {
                this.parentWindow.DialogResult = false;
            }
        }

        /// <summary>
        /// Maps Esc to the cancel button and Enter to the OK button.
        /// </summary>
        private void RegistrationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.CancelButton_Click(sender, e);
            }
            else if (e.Key == Key.Enter && this.registerButton.IsEnabled)
            {
                this.RegisterButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Sets focus to the user name text box.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }

        /// <summary>
        /// when the foundry combo box changed load customer  combo box for selected foundry        
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private void foundryComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.roleCombobox.SelectedValue != null)
                if (this.roleCombobox.SelectedValue.ToString().ToLower() != "customer")
                    this.customerCombobox.IsEnabled = false;

            radiologyContext.Load(radiologyContext.GetCustomersFilteredQuery(foundryCombobox.SelectedValue.ToString())).Completed += CustomerOfFoundry_Loaded;
        }

        void CustomerOfFoundry_Loaded(object sender, EventArgs e)
        {
            CustomerOfFoundry = ((LoadOperation<Customer>)sender).Entities.Select(p => p.CustomerName);
            this.customerCombobox.ItemsSource = CustomerOfFoundry;

        }

        private void roleComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedValue.ToString().ToLower() != "customer")
            {
                this.customerCombobox.SelectedItem = null;
                this.customerCombobox.IsEnabled = false;
            }
            else
                this.customerCombobox.IsEnabled = true;

        }
    }
}
