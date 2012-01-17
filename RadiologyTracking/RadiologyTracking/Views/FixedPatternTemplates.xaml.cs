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
using System.Windows.Navigation;
using RadiologyTracking.Web.Models;
using Vagsons.Controls;
using System.Windows.Data;
using RadiologyTracking.Web.Services;
using System.Collections;

namespace RadiologyTracking.Views
{
    public partial class FixedPatternTemplates : BaseCRUDView
    {
        public FixedPatternTemplates()
        {
            InitializeComponent();
            if (App.FixedPattern == null)
            {
                //Means landed here by mistake, just go back to fixed patterns page
                Navigate("/FixedPatterns");
            }
            else
            {
                this.FixedPattern = App.FixedPattern;
                DataContext = this.FixedPattern;
            }

            //wire up event handlers
            AddEventHandlers();
            SetBindings();
        }

        /// <summary>
        /// Adds the required event handlers for all the page elements
        /// </summary>
        private void AddEventHandlers()
        {
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;
            btnCancel.Click += CancelOperation;
            btnSave.Click += SaveOperation;
            FPTemplateRowsSource.LoadedData += FPTemplateRowsSource_LoadedData;
        }

        /// <summary>
        /// Sets the bindings of some properties to some of the UI elements, since they can't be bound directly in XAML. 
        /// </summary>
        private void SetBindings()
        {
            txtFPNo.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("FixedPattern.FPNo") });
            txtDescription.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("FixedPattern.Description") });
            txtCustomer.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("FixedPattern.Customer.ShortName") });
            txtFPTemplateID.SetBinding(TextBlock.TextProperty, new Binding() { Source = this, Path = new PropertyPath("FixedPatternTemplate.ID") });
            FPTemplatesDataGrid.SetBinding(CustomGrid.ItemsSourceProperty, new Binding() { Source = this, Path = new PropertyPath("FPTemplateRows") });
            btnAdd.SetBinding(Button.IsEnabledProperty, new Binding() { Source = this, Path = new PropertyPath("Enabled") });
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return this.FPTemplatesDataGrid; }
        }

        public override DomainDataSource DomainSource
        {
            get { return this.FPTemplatesSource; }
        }

        public override Type MainType
        {
            get { return typeof(FPTemplateRow); }
        }

        private FixedPattern _fixedPattern;

        /// <summary>
        /// Current Fixed Pattern for whom templates are being created currently
        /// </summary>
        public FixedPattern FixedPattern
        {
            get
            {
                return this._fixedPattern;
            }
            set
            {
                this._fixedPattern = value;
                OnPropertyChanged("FixedPattern");
            }
        }


        public bool Enabled
        {
            get
            {
                return !(FixedPatternTemplate == null);
            }
        }

        private FixedPatternTemplate _fixedPatternTemplate;

        /// <summary>
        /// Current Fixed Pattern Template
        /// </summary>
        public FixedPatternTemplate FixedPatternTemplate
        {
            get
            {
                return this._fixedPatternTemplate;
            }
            set
            {
                this._fixedPatternTemplate = value;
                OnPropertyChanged("FixedPatternTemplate");
                OnPropertyChanged("Enabled");
            }
        }

        //kept ienumerable, so that the loaded object from the datacontext can be directly assigned here
        private IEnumerable _fPTemplateRows;

        public IEnumerable FPTemplateRows
        {
            get
            {
                return _fPTemplateRows;
            }
            set
            {
                _fPTemplateRows = value;
                OnPropertyChanged("FPTemplateRows");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DomainSource.Load();
        }

        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            //also give a few default empty string values so that UI copy operation is possible
            FPTemplateRow FPTemplateRow = new FPTemplateRow()
                                            {
                                                FixedPatternTemplate = this.FixedPatternTemplate,
                                                //auto increment sl no for each additional row
                                                SlNo = ((DomainDataSourceView)FPTemplateRows).Count + 1,
                                                Density = " ",
                                                Designation = " ",
                                                Location = " ",
                                                Segment = " ",
                                                Sensitivity = " ",
                                                FilmSizeString = " "
                                            };

            ((DomainDataSourceView)FPTemplateRows).Add(FPTemplateRow);
            OnPropertyChanged("FPTemplateRows");
        }

        public override void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            base.domainDataSource_LoadedData(sender, e);
            //first item returned is the current fixed pattern template for the given combination of fixed pattern and coverage
            FixedPatternTemplate = (FixedPatternTemplate)((DomainDataSourceView)((DomainDataSource)sender).Data).GetItemAt(0);
            FPTemplateRowsSource.Load();
        }

        void FPTemplateRowsSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            FPTemplateRows = (DomainDataSourceView)((DomainDataSource)sender).Data;
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }
    }
}