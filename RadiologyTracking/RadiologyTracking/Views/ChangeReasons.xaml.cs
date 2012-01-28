using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using RadiologyTracking.Web.Models;
using System.ComponentModel;
using System.ServiceModel.DomainServices.Client;

namespace RadiologyTracking.Views
{
    public partial class ChangeReasons : ChildWindow, INotifyPropertyChanged
    {
        IEnumerable<Change> _changes = new List<Change>();

        public IEnumerable<Change> Changes
        {
            get
            {
                return this._changes;
            }
            set
            {
                this._changes = value;
                this.OnPropertyChanged("Changes");
                this.ChangesGrid.ItemsSource = Changes;
            }
        }

        public ChangeReasons()
        {
            InitializeComponent();
            this.ChangesGrid.ItemsSource = Changes;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (ChangesGrid.CommitEdit())
            {
                foreach(var change in Changes)
                {
                    if (String.IsNullOrEmpty(change.Why.Trim()))
                    {
                        //show validation error
                        MessageBox.Show("Please fill the reasons for all the changes");
                        return;
                    }
                }
                
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

