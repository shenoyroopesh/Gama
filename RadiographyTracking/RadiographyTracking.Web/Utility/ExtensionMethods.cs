using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Web.Security;
using System.Web.Profile;

namespace RadiographyTracking.Web.Utility
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension method for Enum
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        /// <summary>
        /// This extension method allows a membership user to create and return a corresponding User instance
        /// </summary>
        /// <param name="membershipUser"></param>
        /// <returns></returns>
        public static User GetUser(this MembershipUser membershipUser)
        {
            User user = new User()
            {
                Name = membershipUser.UserName
            };
            ProfileBase p = ProfileBase.Create(user.Name);
            user.Foundry = (string)p.GetPropertyValue("Foundry");
            user.FriendlyName = (string)p.GetPropertyValue("FriendlyName");
            return user;
        }

        /// <summary>
        /// Performs a shallow copy of all same named public properties from source to destination. Does not touch private or protected
        /// properties
        /// 
        /// While performing a copy, if there are other complex objects, it just copies the reference and not create a new
        /// instance of the property type
        /// </summary>
        /// <param name="source">Source from where to copy the object</param>
        /// <param name="destination">Destination object to be copied to</param>
        /// <param name="excludeProperties">Comma separated names of properties that should not be copied. Also if entity names are included, 
        /// then corresponding foreign key columns with name [entity]ID are also excluded</param>
        /// <returns></returns>
        public static void CopyTo(this Object source, Object destination, string excludeProperties)
        {
            Type SourceType = source.GetType();
            Type DestinationType = destination.GetType();
            string[] excluded = null;

            if (!String.IsNullOrEmpty(excludeProperties))
            {
                excluded = excludeProperties.Split(',');
            }

            PropertyInfo[] properties = DestinationType.GetProperties();

            foreach (var destProperty in properties)
            {
                if ((!destProperty.CanWrite) || (excluded != null && excluded.Contains(destProperty.Name)))
                    continue;

                //for eg, if Welder is excluded, make sure that WelderID is also not copied without having to explicitly exclude WelderID
                if (excluded != null && excluded.Contains(destProperty.Name.Replace("ID", ""))) 
                    continue;

                var sourceProperty = SourceType.GetProperty(destProperty.Name);

                if(sourceProperty == null || !sourceProperty.CanRead)
                    continue;

                destProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);                
            }
        }
    }
}