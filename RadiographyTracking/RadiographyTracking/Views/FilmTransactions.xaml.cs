using System;
using System.Windows;
using System.Windows.Controls;
using RadiographyTracking.Web.Models;
using Vagsons.Controls;

namespace RadiographyTracking.Views
{
    public partial class FilmTransactions : BaseCRUDView
    {
        public FilmTransactions()
            : base()
        {
            InitializeComponent();
            DomainSource.LoadedData += domainDataSource_LoadedData;
            btnAdd.Click += AddOperation;
            btnCancel.Click += CancelOperation;
            btnSave.Click += SaveOperation;

            fromDatePicker.SelectedDate = fromDatePicker.DisplayDate = DateTime.Now.AddDays(-15);
            toDatePicker.SelectedDate = toDatePicker.DisplayDate = DateTime.Now;
            ConsiderAdditions = false;
        }

        [CLSCompliant(false)]
        public override CustomGrid Grid
        {
            get { return filmTransactionsDataGrid; }
        }

        public override DomainDataSource DomainSource
        { 
            get { return filmTransactionsDomainDataSource; } 
        }

        public override Type MainType
        {
            get { return typeof(FilmTransaction); }
        }

        public override String ChangeContext
        {
            get
            {
                return "Film Transaction";
            }
        }

        public override String ChangeContextProperty
        {
            get
            {
                return "ChallanNo";
            }
        }

        //Kept here only for the template column to work fine
        public override void DeleteOperation(object sender, RoutedEventArgs e)
        {
            base.DeleteOperation(sender, e);
        }

        /// <summary>
        /// Overrides the addoperation from the baseview, so that the film transaction is instantiated with default date
        /// of today
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void AddOperation(object sender, RoutedEventArgs e)
        {
            ((DomainDataSourceView)Grid.ItemsSource).Add(new FilmTransaction() { Date = DateTime.Now });
        }
    }
}
