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
using Vagsons.Controls;
using System.ComponentModel;

namespace RadiologyTracking.Views
{
    /// <summary>
    /// Note: This is not abstract only to ensure that VS designer support is not broken.
    /// 
    /// DO NOT USE THIS VIEW DIRECTLY
    /// </summary>
    public class BaseCRUDView : UserControl, INotifyPropertyChanged
    {
        public BaseCRUDView()
        {
            //only here for VS designer support
        }

        /// <summary>
        /// Represents the main grid in this page, use the new keyword to hide this and define one corresponding to the page
        /// </summary>
        [CLSCompliant(false)]
        public virtual CustomGrid Grid { get { return null; } }

        /// <summary>
        /// Represents the main domaindatasource in this page, use the new keyword to hide this and define one corresponding to the page
        /// </summary>
        public virtual DomainDataSource DomainSource { get { return null; } }

        /// <summary>
        /// Represents the main item type in this page, use the new keyword to hide this and define one corresponding to the page
        /// </summary>
        public virtual Type MainType { get { return null; } }


        /// <summary>
        /// Property to find and get the Frame for this user control, mainly for navigation
        /// </summary>
        public Frame Frame
        {
            get
            {
                return ((Frame)Utility.GetParent(this, typeof(Frame)));
            }
        }

        public void Navigate(string uri)
        {
            Frame.Navigate(new Uri(uri, UriKind.Relative));
        }


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

        /// <summary>
        /// This handles the load of the datasource data and shows errors if any
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// This handles the add operation for this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void AddOperation(object sender, RoutedEventArgs e)
        {
            ((DomainDataSourceView)Grid.ItemsSource).Add(Activator.CreateInstance(MainType));
        }


        /// <summary>
        /// This handles the save operation for this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void SaveOperation(object sender, RoutedEventArgs e)
        {
            if (Grid.IsValid)
                //commit any unsaved changes to avoid an exception
                if (Grid.CommitEdit())
                    DomainSource.DomainContext.SubmitChanges(BaseCRUDView.OnFormSubmitCompleted, null);
        }

        /// <summary>
        /// This handles the delete operation for this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void DeleteOperation(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if (Grid.CommitEdit())
                ((DomainDataSourceView)Grid.ItemsSource).Remove(row.DataContext);
        }

        /// <summary>
        /// This handles the cancel operation for this page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CancelOperation(object sender, RoutedEventArgs e)
        {
            if (Grid.CommitEdit())
                DomainSource.DomainContext.RejectChanges();
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
