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
using RadiographyTracking.Web.Models;
using Vagsons.Controls;
using System.Windows.Data;
using RadiographyTracking.Web.Services;
using System.Collections;
using System.ServiceModel.DomainServices.Client;

namespace RadiographyTracking.Views
{
    public partial class FixedPatternTemplates : BaseCRUDView
    {
        public FixedPatternTemplates()
            : base()
        {
            InitializeComponent();
            this.ExcludePropertiesFromTracking.Add("FixedPatternTemplate");
            this.ExcludePropertiesFromTracking.Add("FixedPatternTemplateID");
            this.ExcludePropertiesFromTracking.Add("FPTemplateRows");

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

        public override String ChangeContext
        {
            get
            {
                return "Fixed Pattern Template";
            }
        }

        public override String ChangeContextValue
        {
            get
            {
                return this.txtFPNo.Text + " " + (this.cmbCoverage.SelectedItem as Coverage).CoverageName;
            }
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
        private EntityCollection<FPTemplateRow> _fPTemplateRows;

        public EntityCollection<FPTemplateRow> FPTemplateRows
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
                                                SlNo = FPTemplateRows.Max(p => p.SlNo) + 1,
                                                Density = " ",
                                                Designation = " ",
                                                Location = " ",
                                                Segment = " ",
                                                Sensitivity = " ",
                                                FilmSizeString = " "
                                            };

            FPTemplateRows.Add(FPTemplateRow);
            OnPropertyChanged("FPTemplateRows");
        }

        public override void domainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            base.domainDataSource_LoadedData(sender, e);
            //first item returned is the current fixed pattern template for the given combination of fixed pattern and coverage
            FixedPatternTemplate = (FixedPatternTemplate)((DomainDataSourceView)((DomainDataSource)sender).Data).GetItemAt(0);
            FPTemplateRows = FixedPatternTemplate.FPTemplateRows;
        }

        /// <summary>
        /// Overridden because the datasource for the grid is not a domaindataview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Confirm Delete", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            DataGridRow row = DataGridRow.GetRowContainingElement(sender as FrameworkElement);
            //commit any unsaved changes to avoid an exception
            if (Grid.CommitEdit())
            {                
                var rowToBeRemoved = (FPTemplateRow)row.DataContext;
                FPTemplateRows.Remove(rowToBeRemoved);
            }
        }

        public override void SaveOperation(object sender, RoutedEventArgs e)
        {
            //validations on each row
            foreach (var row in FPTemplateRows)
            {
                //sl no should be unique
                var duplicateRow = FPTemplateRows.FirstOrDefault(p => p.SlNo == row.SlNo && p != row);
                if (duplicateRow != null)
                {
                    MessageBox.Show("Sl no " + row.SlNo + " has been repeated twice, Correct this before saving");
                    return;
                }

                //Location + segment uniqueness validation validation
                var conflictingRow = FPTemplateRows.FirstOrDefault(p => p.SlNo != row.SlNo && p.Location == row.Location && p.Segment == row.Segment);
                if (conflictingRow != null)
                {
                    MessageBox.Show("Rows with Sl No " + row.SlNo + " and " + conflictingRow.SlNo + " have the same location and segments. Correct this before saving");
                    return;
                }
            }

            base.SaveOperation(sender, e);
        }


        /// <summary>
        /// Have to update filmsize string manually
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowFilmSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            var row = ((DataGridCell)cmb.Parent).DataContext;
            ((FPTemplateRow)row).FilmSizeString = ((FilmSize)cmb.SelectedValue).Name;
        }
    }
}