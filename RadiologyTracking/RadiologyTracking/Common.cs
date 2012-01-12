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
using System.ServiceModel.DomainServices.Client;

namespace RadiologyTracking
{
    public class Common
    {
        /// <summary>
        /// This is used to handle the domaincontext submit operation result, and check whether there are any errors. If not, user is show success message
        /// 
        /// Since it is common to all views, instead of repeating everywhere it is created as a single function
        /// </summary>
        /// <param name="so"></param>
        public static void OnFormSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                MessageBox.Show(string.Format("Submit Failed: {0}", so.Error.Message), "Error", MessageBoxButton.OK);
                so.MarkErrorAsHandled();
            }
            else
            {
                MessageBox.Show("Saved Successfully", "Success", MessageBoxButton.OK);
            }
        }
    }
}
