using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using RadiographyTracking.Web.Models;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

namespace RadiographyTracking
{
    public class Utility
    {
        /// <summary>
        /// Recursive method to find the top level parent of a given type for a particular object
        /// </summary>
        /// <param name="o"></param>
        /// <param name="parentType"></param>
        /// <returns></returns>
        public static Object GetParent(Object o, Type parentType)
        {
            Type objectType = o.GetType();
            PropertyInfo p = objectType.GetProperty("Parent");
            if (p != null)
            {
                object Parent = p.GetValue(o, null);
                if (Parent == null || Parent.GetType() == parentType)
                    return Parent;
                else
                    return GetParent(Parent, parentType);
            }
            else
            {
                //means no parent in this chain
                return null;
            }
        }


        public static List<Change> GetChanges<T>(T oldEntity, T newEntity, string changeContext, 
                                                string user, List<String> ExcludedProperties)
        {
            Type type = oldEntity.GetType();
            List<Change> changes = new List<Change>();

            //check each property for change
            foreach (var property in type.GetProperties())
            {
                //avoid comparing ids for associated ids, depend on properties
                if(property.Name.Contains("ID"))
                    continue;

                //check excluded properties
                if (ExcludedProperties != null && ExcludedProperties.Contains(property.Name))
                    continue;

                //avoid checking property from Entity types
                if (typeof(Entity).GetProperty(property.Name) != null)
                    continue;

                var oldPropertyValue = property.GetValue(oldEntity, null);
                var newPropertyValue = property.GetValue(newEntity, null);

                //do not track adding new values, only modification and deletion of values
                if (oldPropertyValue == null || String.IsNullOrEmpty(oldPropertyValue.ToString().Trim()))
                    continue;

                //Convention - for combobox based fields, corresponding field might be present as Text
                //change in text will always be there, but in combobox it may or may not be there, so give
                //preference to Text, but remove "Text" part to make it more meaningful

                bool hasText = false;

                foreach (var prop in type.GetProperties())
                {
                    if (String.Concat(property.Name.ToLower(), "text").Equals(prop.Name.ToLower()) ||
                        String.Concat(property.Name.ToLower(), "string").Equals(prop.Name.ToLower()))
                    {
                        hasText = true;
                        break;
                    }
                }

                if (hasText)
                    continue;

                if (!((newPropertyValue ?? new object()).Equals(oldPropertyValue)))
                {
                    changes.Add(new Change()
                                    {
                                        What = property.Name.Replace("Text", "").Replace("String", ""),
                                        Where = changeContext,
                                        FromValue = (oldPropertyValue ?? "").ToString(),
                                        ToValue = (newPropertyValue ?? "").ToString(),
                                        Why = " ", //this will be given by the user
                                        When = DateTime.Now,
                                        ByWhom = user
                                    });
                }
            }

            return changes;
        }
    }
}