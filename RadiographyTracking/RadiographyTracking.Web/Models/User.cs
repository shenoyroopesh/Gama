﻿namespace RadiographyTracking.Web
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
using RadiographyTracking.Web.Models;

    /// <summary>
    /// Class containing information about the authenticated user.
    /// </summary>
    public partial class User : UserBase
    {
        //// NOTE: Profile properties can be added for use in Silverlight application.
        //// To enable profiles, edit the appropriate section of web.config file.
        ////
        //// public string MyProfileProperty { get; set; }
        /// <summary>
        /// Gets and sets the friendly name of the user.
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Used for identifying which Foundry this user belongs to
        /// </summary>
        public string Foundry { get; set; }

        /// <summary>
        /// Used for identifying customer company in case user's role is Customer
        /// </summary>
        public string CustomerCompany { get; set; }
    }
}
