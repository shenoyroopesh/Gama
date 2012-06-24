namespace RadiographyTracking.Web
{
    using System.ComponentModel.DataAnnotations;
    using RadiographyTracking.Web.Resources;

    /// <summary>
    /// Class containing the values and validation rules for user registration.
    /// </summary>
    public sealed partial class RegistrationData
    {
        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 0, Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName = "ValidationErrorInvalidUserName", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "ValidationErrorBadUserNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string UserName { get; set; }


        /// <summary>
        /// Gets and sets the friendly name of the user.
        /// </summary>
        [Display(Order = 1, Name = "FriendlyNameLabel", Description = "FriendlyNameDescription", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(255, MinimumLength = 0, ErrorMessageResourceName = "ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string FriendlyName { get; set; }


        [Display(Order=4, Name="Foundry")]
        public string Foundry { get; set; }

        /// <summary>
        /// Role to which the user should be assigned. In this application, every user will have only a single role
        /// </summary>
        [Display(Order = 5, Name= "Role")]
        public string Role { get; set; }
    }
}
