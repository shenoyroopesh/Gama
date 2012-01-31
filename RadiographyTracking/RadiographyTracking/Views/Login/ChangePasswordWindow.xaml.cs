namespace RadiographyTracking.LoginUI
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// <see cref="ChildWindow"/> class that controls the registration process.
    /// </summary>
    public partial class ChangePasswordWindow : ChildWindow
    {
        private IList<OperationBase> possiblyPendingOperations = new List<OperationBase>();

        /// <summary>
        /// Creates a new <see cref="LoginRegistrationWindow"/> instance.
        /// </summary>
        public ChangePasswordWindow()
        {
            InitializeComponent();
            this.changePasswordForm.SetParentWindow(this);
        }

        /// <summary>
        /// Initializes the <see cref="VisualStateManager"/> for this component by putting it into the "AtLogin" state.
        /// </summary>
        private void GoToInitialState(object sender, EventArgs eventArgs)
        {
            this.LayoutUpdated -= this.GoToInitialState;
            VisualStateManager.GoToState(this, "AtLogin", true);
        }

        /// <summary>
        /// Ensures the visual state and focus are correct when the window is opened.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            this.NavigateToLogin();
        }

        /// <summary>
        /// Notifies the <see cref="LoginRegistrationWindow"/> window that it can only close if <paramref name="operation"/> is finished or can be cancelled.
        /// </summary>
        /// <param name="operation">The pending operation to monitor</param>
        public void AddPendingOperation(OperationBase operation)
        {
            this.possiblyPendingOperations.Add(operation);
        }

        /// <summary>
        /// Causes the <see cref="VisualStateManager"/> to change to the "AtLogin" state.
        /// </summary>
        public virtual void NavigateToLogin()
        {
            //VisualStateManager.GoToState(this, "AtLogin", true);
            this.changePasswordForm.SetInitialFocus();
        }

        /// <summary>
        /// Prevents the window from closing while there are operations in progress
        /// </summary>
        private void ChangePasswordWindow_Closing(object sender, CancelEventArgs eventArgs)
        {
            foreach (OperationBase operation in this.possiblyPendingOperations)
            {
                if (!operation.IsComplete)
                {
                    if (operation.CanCancel)
                    {
                        operation.Cancel();
                    }
                    else
                    {
                        eventArgs.Cancel = true;
                    }
                }
            }
        }
    }
}