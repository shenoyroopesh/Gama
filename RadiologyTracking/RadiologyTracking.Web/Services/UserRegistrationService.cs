﻿namespace RadiologyTracking.Web
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Web.Profile;
    using System.Web.Security;
    using RadiologyTracking.Web.Resources;
    using System.Linq;
    using RadiologyTracking.Web.Utility;

    // TODO: Switch to a secure endpoint when deploying the application.
    //       The user's name and password should only be passed using https.
    //       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       More information on using https with a Domain Service can be found on MSDN.

    /// <summary>
    /// Domain Service responsible for registering users.
    /// </summary>
    [EnableClientAccess, RequiresAuthentication, RequiresRole("admin")]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// Role to which users will be added by default.
        /// </summary>
        public const string DefaultRole = "Registered Users";
        
        /// <summary>
        /// This method gets all the registration data for the users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RegistrationData> GetUsers()
        {
            var users = Membership.GetAllUsers();
            List<RegistrationData> registeredUsers = new List<RegistrationData>();
            foreach (MembershipUser mUser in users)
            {
                var user = mUser.GetUser();
                RegistrationData data = new RegistrationData()
                                        {
                                            UserName = user.Name,
                                            Role = Roles.GetRolesForUser(user.Name).FirstOrDefault(),
                                            Foundry = user.Foundry,
                                            FriendlyName = user.FriendlyName
                                        };
                registeredUsers.Add(data);
            }
            return registeredUsers;
        }

        /// <summary>
        /// Adds a new user with the supplied <see cref="RegistrationData"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="user">The registration information for this user.</param>
        /// <param name="password">The password for the new user.</param>
        [Invoke(HasSideEffects = true)]
        [RequiresAuthentication]
        [RequiresRole("admin")]
        public void CreateUser(RegistrationData user,
            [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            [RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName = "ValidationErrorBadPasswordStrength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            [StringLength(50, MinimumLength = 7, ErrorMessageResourceName = "ValidationErrorBadPasswordLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Run this BEFORE creating the user to make sure roles are enabled and the default role is available.
            //
            // If there is a problem with the role manager, it is better to fail now than to fail after the user is created.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // NOTE: ASP.NET by default uses SQL Server Express to create the user database. 
            // CreateUser will fail if you do not have SQL Server Express installed.
            Membership.CreateUser(user.UserName, password);

            // Assign the user to the default role if the given role does not exist
            // This will fail if role management is disabled.
            if (Roles.RoleExists(user.Role))
                Roles.AddUserToRole(user.UserName, user.Role);
            else 
                Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // Set the friendly name (profile setting).
            // This will fail if the web.config is configured incorrectly.
            ProfileBase profile = ProfileBase.Create(user.UserName, false);
            profile.SetPropertyValue("FriendlyName", user.FriendlyName);
            profile.SetPropertyValue("Foundry", user.Foundry);
            profile.Save();
        }

        [RequiresAuthentication, RequiresRole("admin")]
        public void EditUser(RegistrationData user, 
            [RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName = "ValidationErrorBadPasswordStrength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            [StringLength(50, MinimumLength = 7, ErrorMessageResourceName = "ValidationErrorBadPasswordLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            string password = "")
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Run this BEFORE creating the user to make sure roles are enabled and the default role is available.
            //
            // If there is a problem with the role manager, it is better to fail now than to fail after the user is created.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }
            MembershipUser membershipUser = Membership.GetUser(user.UserName);

            if (password != "")
            {
                String resetPwd = membershipUser.ResetPassword();
                membershipUser.ChangePassword(resetPwd, password);
            }

            if (Roles.RoleExists(user.Role))
            {
                foreach (var role in Roles.GetRolesForUser(user.UserName))
                {
                    Roles.RemoveUserFromRole(user.UserName, role);
                }
                Roles.AddUserToRole(user.UserName, user.Role);
            }

            // Set the friendly name (profile setting).
            // This will fail if the web.config is configured incorrectly.
            ProfileBase profile = ProfileBase.Create(user.UserName, false);
            profile.SetPropertyValue("FriendlyName", user.FriendlyName);
            profile.SetPropertyValue("Foundry", user.Foundry);
            profile.Save();
        }

        [RequiresAuthentication, RequiresRole("admin")]
        public bool DeleteUser(String userName)
        {
            return Membership.DeleteUser(userName);
        }
    }

    /// <summary>
    /// An enumeration of the values that can be returned from <see cref="UserRegistrationService.CreateUser"/>
    /// </summary>
    public enum CreateUserStatus
    {
        Success = 0,
        InvalidUserName = 1,
        InvalidPassword = 2,
        InvalidQuestion = 3,
        InvalidAnswer = 4,
        InvalidEmail = 5,
        DuplicateUserName = 6,
        DuplicateEmail = 7,
        Failure = 8,
    }
}