using System;
using System.Collections.Generic;
using System.Linq;
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
            // Get fieldinfo for this type
            var fieldInfo = value.GetType().GetField(value.ToString());

            // Get the stringvalue attributes
            var attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];

            // Return the first if there was a match.
            return attribs != null && attribs.Length > 0 ? attribs[0].StringValue : null;
        }

        /// <summary>
        /// This extension method allows a membership user to create and return a corresponding User instance
        /// </summary>
        /// <param name="membershipUser"></param>
        /// <returns></returns>
        public static User GetUser(this MembershipUser membershipUser)
        {
            var user = new User()
            {
                Name = membershipUser.UserName
            };
            var p = ProfileBase.Create(user.Name);
            user.Foundry = (string)p.GetPropertyValue("Foundry");
            user.FriendlyName = (string)p.GetPropertyValue("FriendlyName");
            user.CustomerCompany = (string)p.GetPropertyValue("CustomerCompany");
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
            var sourceType = source.GetType();
            var destinationType = destination.GetType();
            string[] excluded = null;

            if (!String.IsNullOrEmpty(excludeProperties))
                excluded = excludeProperties.Split(',');

            var properties = destinationType.GetProperties();

            foreach (var destProperty in properties)
            {
                if ((!destProperty.CanWrite) || (excluded != null && excluded.Contains(destProperty.Name)))
                    continue;

                //for eg, if Welder is excluded, make sure that WelderID is also not copied without having to explicitly exclude WelderID
                if (excluded != null && excluded.Contains(destProperty.Name.Replace("ID", ""))) 
                    continue;

                var sourceProperty = sourceType.GetProperty(destProperty.Name);

                if(sourceProperty == null || !sourceProperty.CanRead)
                    continue;

                destProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);                
            }
        }

        /// <summary>
        /// Splits the observations into albhabetical and numerical components
        /// </summary>
        /// <param name="input"></param>
        /// <returns>array of strings with two strings - first element for alphabet components, second one for numeric ones</returns>
        public static Tuple<string, string> SplitObservation(this string input)
        {
            var observations = input.Split(',').Select(p => p.Trim());

            var results = (from observation in observations
                           let indexOfFirstNumber = observation.IndexOfAny("0123456789".ToCharArray())
                           select
                               new Tuple<string, string>(
                               observation.Substring(0, indexOfFirstNumber < 0 ? observation.Length : indexOfFirstNumber),
                               indexOfFirstNumber < 0 ? "" : observation.Substring(indexOfFirstNumber))).ToList();

            return
                new Tuple<string, string>(
                    String.Join(",", results.Select(p => p.Item1)),
                    String.Join(",", results.Select(p => p.Item2)
                    //This filter is needed to ensure that if number is not present it doesn't
                    //cause stray commas
                                         .Where(q => !String.IsNullOrEmpty(q))));
        }
    }
}