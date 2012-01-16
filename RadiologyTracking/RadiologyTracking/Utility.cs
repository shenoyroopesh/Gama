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

namespace RadiologyTracking
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
    }
}
