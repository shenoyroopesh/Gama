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
using System.Windows.Data;
using System.Collections.Generic;
using RadiologyTracking.Web.Models;
using RadiologyTracking.Web.Services;
using System.Reflection;

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
            //try-catch added mainly for design-time support, else the design view of pages inheriting this class would
            //throw an error
            try
            {
                var roles = WebContext.Current.User.Roles as String[];
                string currentRole = (WebContext.Current.User.Roles as String[])[0];
                //for clerk no editing
                CustomGrid.IsEditAllowed = !(currentRole.ToLower() == "clerk");
                CustomGrid.ExcludePropertiesFromTracking = this.ExcludePropertiesFromTracking;
            }
            catch
            {

            }
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
        /// Represents the change context for this page, use the new keyword to hide this and define one specific to your page
        /// </summary>
        public virtual String ChangeContext { get { return null; } }

        /// <summary>
        /// Represents the change context Property for this page, use the new keyword to hide this and define one specific to your page
        /// </summary>
        public virtual String ChangeContextProperty { get { return null; } }


        /// <summary>
        /// Optional value for the entire page - if this is set, ChangeContextProperty will not be used to dynamically
        /// pick the value from the changed object for reporting purpose
        /// </summary>
        public virtual String ChangeContextValue { get { return null; } }


        private Dictionary<int, object> _originalEntities = new Dictionary<int,object>();

        /// <summary>
        /// Original Entities tracked at page level
        /// </summary>
        public Dictionary<int, object> OriginalEntities 
        {
            get
            {
                return this._originalEntities;
            }
            set
            {
                this._originalEntities = value;
            }
        }


        public List<String> ExcludePropertiesFromTracking = new List<String>();

        
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
        public virtual void OnFormSubmitCompleted(SubmitOperation so)
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
            string user = WebContext.Current.User.DisplayName;
            if (Grid.IsValid)
            {
                //commit any unsaved changes to avoid an exception
                if (Grid.CommitEdit())
                {
                    List<Change> allChanges = new List<Change>();

                    //capture all the changes and their reasons
                    var modification = DomainSource.DomainContext.EntityContainer.GetChanges();

                    #region Change tracking for modified entries
                    
                    foreach (var modified in modification.ModifiedEntities)
                    {
                        //var original = modified.GetOriginal();
                        object original;
                        Grid.OriginalEntities.TryGetValue((int)modified.GetIdentity(), out original);

                        //also try in this page level modified entries
                        if(original == null && OriginalEntities != null)
                        {
                            object pagelevelOriginal;                            
                            this.OriginalEntities.TryGetValue((int)modified.GetIdentity(), out pagelevelOriginal);

                            //this is assuming that page level there will be only one entity type at max. This is a reasonable
                            //assumption for this application
                            if (pagelevelOriginal.GetType() == modified.GetType())
                                original = pagelevelOriginal;
                        }

                        if (original != null)
                        {
                            Type type = modified.GetType();

                            PropertyInfo changeContextProp = null;

                            if (ChangeContextValue == null)
                            {
                                changeContextProp = type.GetProperty(ChangeContextProperty);
                            }

                            //check if change context value is set, else take it from the change context property name defined
                            var propertyValue = this.ChangeContextValue ?? 
                                                changeContextProp.GetValue(modified, null);
                            string changeContextString = String.Concat(ChangeContext, "-",
                                                                       (propertyValue ?? "").ToString());
                            List<Change> changes = Utility.GetChanges(original, modified, changeContextString, 
                                                                        user, ExcludePropertiesFromTracking);
                            changes.ForEach(p => allChanges.Add(p));
                        }
                    }

                    #endregion

                    #region change tracking for deleted entries                   

                    foreach (var deleted in modification.RemovedEntities)
                    {
                        Type type = deleted.GetType();
                        PropertyInfo changeContextProp = type.GetProperty(ChangeContextProperty);
                        var propertyValue = this.ChangeContextValue ?? changeContextProp.GetValue(deleted, null);
                        string changeContextString = String.Concat(ChangeContext, "-",
                                                                   (propertyValue ?? "").ToString());
                        Change change = new Change()
                                        {
                                            When = DateTime.Now,
                                            Where = changeContextString,
                                            FromValue = "",
                                            ToValue= "Deleted",
                                            ByWhom = user,
                                            Why = " "
                                        };

                        allChanges.Add(change);
                    }

                    #endregion

                    #region change tracking for added entries
                    //for added entities, whether changes will be tracked or not is determined by the page level
                    //hence keeping this there

                    foreach (var modified in modification.AddedEntities)
                    {
                        Type type = modified.GetType();

                        //do not track addition of changes themselves, which are entities
                        if (type == typeof(Change))
                            continue;

                        PropertyInfo changeContextProp = type.GetProperty(ChangeContextProperty);
                        var propertyValue = changeContextProp.GetValue(modified, null);
                        string changeContextString = String.Concat(ChangeContext, "-",
                                                                    (propertyValue ?? "").ToString());
                        allChanges.Add(new Change()
                                        {
                                            When = DateTime.Now,
                                            Where = changeContextString,
                                            FromValue = "",
                                            ToValue = "Added",
                                            ByWhom = user,
                                            Why = " "
                                        });
                    }

                    #endregion

                    if (allChanges.Count != 0)
                    {
                        //get reason for all the changes - do not let user submit without all reasons
                        ChangeReasons cr = new ChangeReasons();
                        cr.Changes = allChanges;
                        cr.Closed += new EventHandler(cr_Closed);
                        cr.Show();
                    }
                    else
                    {
                        //if no changes worth tracking but still there are changes
                        SaveChanges();
                    }
                    //save all the changes with the reasons
                    foreach (var change in allChanges)
                    {
                        (DomainSource.DomainContext as RadiologyContext).Changes.Add(change);
                    }
                }
            }
        }

        void cr_Closed(object sender, EventArgs e)
        {
            if ((bool)(sender as ChangeReasons).DialogResult)
            {
                SaveChanges();
            }
        }

        void SaveChanges()
        {
            DomainSource.DomainContext.SubmitChanges(OnFormSubmitCompleted, null);
            //make sure the grid saved changes are refreshed for new set of changes
            Grid.ClearOriginalEntities();
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


        public void BindToPage(FrameworkElement element, DependencyProperty prop, String path, BindingMode mode = BindingMode.TwoWay)
        {
            element.SetBinding(prop, new Binding() { Source = this, Path = new PropertyPath(path), Mode = mode });
            OnPropertyChanged(element.Name);
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
