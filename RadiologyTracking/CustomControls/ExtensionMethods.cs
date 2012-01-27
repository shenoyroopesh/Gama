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
using System.ServiceModel.DomainServices.Client;

namespace Vagsons.Controls
{
    public static class ExtensionMethods
    {
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
        public static void CopyTo(this Object source, Object destination)
        {
            Type SourceType = source.GetType();
            Type DestinationType = destination.GetType();

            PropertyInfo[] properties = DestinationType.GetProperties();

            foreach (var destProperty in properties)
            {
                if (!destProperty.CanWrite)
                    continue;

                //don't process properties from entity type
                if (typeof(Entity).GetProperty(destProperty.Name) != null)
                    continue;

                var sourceProperty = SourceType.GetProperty(destProperty.Name);

                if (sourceProperty == null || !sourceProperty.CanRead)
                    continue;

                destProperty.SetValue(destination, sourceProperty.GetValue(source, null), null);
            }
        }

        /// <summary>
        /// Creates a clone and performs a shallow copy of properties from the source object to the destination
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static object Clone(this object source)
        {
            Type t = source.GetType();
            var clone = Activator.CreateInstance(t);
            source.CopyTo(clone);
            return clone;
        }
    }
}
