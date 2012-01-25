namespace RadiologyTracking.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using RadiologyTracking.Web;
    using System.ServiceModel.DomainServices.Client;

    /// <summary>
    /// Form that presents the login fields and handles the login process.
    /// </summary>
    public partial class ChangePassword : StackPanel
    {
        private UserRegistrationContext context= new UserRegistrationContext();
        private ChangePasswordWindow parentWindow;
        private ChangePasswordInfo changePasswordInfo = new ChangePasswordInfo();
        private PasswordBox oldPasswordTextBox;

        /// <summary>
        /// Creates a new <see cref="LoginForm"/> instance.
        /// </summary>
        public ChangePassword()
        {
            InitializeComponent();

            // Set the DataContext of this control to the LoginInfo instance to allow for easy binding.
            this.DataContext = this.changePasswordInfo;
        }

        /// <summary>
        /// Sets the parent window for the current <see cref="LoginForm"/>.
        /// </summary>
        /// <param name="window">The window to use as the parent.</param>
        public void SetParentWindow(ChangePasswordWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Handles <see cref="DataForm.AutoGeneratingField"/> to provide the PasswordAccessor.
        /// </summary>
        private void ChangePasswordForm_AutoGeneratingField(object sender, DataFormAutoGeneratingFieldEventArgs e)
        {
            PasswordBox passwordBox = new PasswordBox();
            e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty);
            if (e.PropertyName == "Password")
            {
                this.changePasswordInfo.PasswordAccessor = () => passwordBox.Password;
            }
            else if (e.PropertyName == "OldPassword")
            {
                this.oldPasswordTextBox = (PasswordBox)e.Field.Content;                
                this.changePasswordInfo.OldPasswordAccessor = () => passwordBox.Password;
            }
            else if (e.PropertyName == "PasswordConfirmation")
            {
                this.changePasswordInfo.PasswordConfirmationAccessor = () => passwordBox.Password;
            }

            //both password boxes need to be replaced

        }

        /// <summary>
        /// Submits the <see cref="LoginOperation"/> to the server
        /// </summary>
        private void OKButton_Click(object sender, EventArgs e)
        {
            // We need to force validation since we are not using the standard OK button from the DataForm.
            // Without ensuring the form is valid, we get an exception invoking the operation if the entity is invalid.
            if (this.changePasswordForm.ValidateItem())
            {
                this.changePasswordInfo.CurrentOperation= context.ChangePassword(this.changePasswordInfo.OldPassword, this.changePasswordInfo.PasswordConfirmation,
                    this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.changePasswordInfo.CurrentOperation);
            }
        }

        /// <summary>
        /// Completion handler for a <see cref="LoginOperation"/>.
        /// If operation succeeds, it closes the window.
        /// If it has an error, it displays an <see cref="ErrorWindow"/> and marks the error as handled.
        /// If it was not canceled, but login failed, it must have been because credentials were incorrect so a validation error is added to notify the user.
        /// </summary>
        private void LoginOperation_Completed(InvokeOperation<bool> op)
        {
            if (op.Value)
            {
                this.parentWindow.DialogResult = true;
            }
            else if (op.HasError)
            {
                MessageBox.Show("Could not change password. Check the current password entered.");
                op.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// If a login operation is in progress and is cancellable, cancel it.
        /// Otherwise, close the window.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (this.changePasswordInfo.CurrentOperation != null && this.changePasswordInfo.CurrentOperation.CanCancel)
            {
                this.changePasswordInfo.CurrentOperation.Cancel();
            }
            else
            {
                this.parentWindow.DialogResult = false;
            }
        }

        /// <summary>
        /// Maps Esc to the cancel button and Enter to the OK button.
        /// </summary>
        private void ChangePasswordForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.CancelButton_Click(sender, e);
            }
            else if (e.Key == Key.Enter)
            {
                this.OKButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Sets focus to the user name text box.
        /// </summary>
        public void SetInitialFocus()
        {
            this.oldPasswordTextBox.Focus();
        }
    }
}
