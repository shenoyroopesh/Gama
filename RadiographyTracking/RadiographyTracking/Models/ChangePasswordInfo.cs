namespace RadiographyTracking.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using RadiographyTracking.Web.Resources;

    /// <summary>
    /// This internal entity is used to ease the binding between the UI controls (DataForm and the label displaying a validation error) and the log on credentials entered by the user.
    /// </summary>
    public class ChangePasswordInfo : ComplexObject
    {
        private OperationBase currentOperation;

                /// <summary>
        /// Gets or sets a function that returns the password.
        /// </summary>
        internal Func<string> OldPasswordAccessor { get; set; }

        /// <summary>
        /// Gets and sets the password.
        /// </summary>
        [Display(Order=0, Name="Old Password")]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string OldPassword
        {
            get
            {
                return (this.OldPasswordAccessor == null) ? string.Empty : this.OldPasswordAccessor();
            }
            set
            {
                this.ValidateProperty("OldPassword", value);

                // Do not store the password in a private field as it should not be stored in memory in plain-text.
                // Instead, the supplied PasswordAccessor serves as the backing store for the value.

                this.RaisePropertyChanged("OldPassword");
            }
        }
        
        /// <summary>
        /// Gets or sets a function that returns the password.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Gets and sets the password.
        /// </summary>
        [Display(Order=1, Name="New Password")]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Password
        {
            get
            {
                return (this.PasswordAccessor == null) ? string.Empty : this.PasswordAccessor();
            }
            set
            {
                this.ValidateProperty("Password", value);

                // Do not store the password in a private field as it should not be stored in memory in plain-text.
                // Instead, the supplied PasswordAccessor serves as the backing store for the value.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Gets or sets a function that returns the password confirmation.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// Gets and sets the password confirmation string.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 2, Name = "PasswordConfirmationLabel", ResourceType = typeof(RegistrationDataResources))]
        public string PasswordConfirmation
        {
            get
            {
                return (this.PasswordConfirmationAccessor == null) ? string.Empty : this.PasswordConfirmationAccessor();
            }

            set
            {
                this.ValidateProperty("PasswordConfirmation", value);
                this.CheckPasswordConfirmation();


                // Do not store the password in a private field as it should not be stored in memory in plain-text.  
                // Instead, the supplied PasswordAccessor serves as the backing store for the value.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// Checks to ensure the password and confirmation match.
        /// If they don't match, a validation error is added.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // If either of the passwords has not yet been entered, then don't test for equality between the fields.
            // The Required attribute will ensure a value has been entered for both fields.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // If the values are different, then add a validation error with both members specified
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// Gets or sets the current registration or login operation.
        /// </summary>
        internal OperationBase CurrentOperation
        {
            get
            {
                return this.currentOperation;
            }
            set
            {
                if (this.currentOperation != value)
                {
                    if (this.currentOperation != null)
                    {
                        this.currentOperation.Completed -= (s, e) => this.CurrentOperationChanged();
                    }

                    this.currentOperation = value;

                    if (this.currentOperation != null)
                    {
                        this.currentOperation.Completed += (s, e) => this.CurrentOperationChanged();
                    }

                    this.CurrentOperationChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user is presently being logged in.
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool IsChanging
        {
            get
            {
                return this.CurrentOperation != null && !this.CurrentOperation.IsComplete;
            }
        }

        /// <summary>
        /// Helper method for when the current operation changes.
        /// Used to raise appropriate property change notifications.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsChanging");
        }
    }
}
