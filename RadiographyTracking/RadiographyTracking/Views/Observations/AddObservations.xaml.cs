namespace RadiographyTracking.Observations
{
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
    using Vagsons.Controls;
    using RadiographyTracking.Web.Models;
    using RadiographyTracking.Web.Services;
    using System.ServiceModel.DomainServices.Client;

    public partial class AddObservations : ChildWindow
    {
        public event EventHandler SubmitClicked;

        public AddObservations()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MultipleObservations = string.Join(",", ObservatonCollection.Where(p => p.IsChecked == true).Select(p => p.Value));

            if (!string.IsNullOrEmpty(MultipleObservations))
            {
                if (!string.IsNullOrEmpty(txtOldObservations.Text.Trim()))
                    MultipleObservations = string.Join(",", txtOldObservations.Text.Trim(), MultipleObservations);
            }
            else
                MultipleObservations = txtOldObservations.Text.Trim();

            if (SubmitClicked != null)
            {
                this.DialogResult = true;
                SubmitClicked(this, new EventArgs());
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public DomainDataSource DomainSource
        {
            get { return this.observationsDataSource; }
        }

        [CLSCompliant(false)]
        public CustomGrid Grid
        {
            get { return this.observationsGrid; }
        }

        public string MultipleObservations { get; set; }
        public int ReportID { get; set; }

        /// <summary>
        /// Ensures the visual state and focus are correct when the window is opened.
        /// </summary>
        protected override void OnOpened()
        {
            base.OnOpened();
            observationsGrid.ItemsSource = GetListOfObservations();
        }

        private List<Observation> ObservatonCollection;

        public List<Observation> GetListOfObservations()
        {
            List<string> listOfObserations = this.MultipleObservations.Split(',').ToList();
            var ctx = (RadiographyContext)this.DomainSource.DomainContext;
            ctx.Load(ctx.GetObservationsQuery());
            ObservatonCollection = new List<Observation>();
            List<string> listOfObserationsFromDB = ctx.Observations.Select(p => p.Value).ToList();

            var oldObservations = string.Join(",", listOfObserations.Except(listOfObserationsFromDB).ToList());
            txtOldObservations.Text = oldObservations;

            if (string.IsNullOrEmpty(txtOldObservations.Text))
            {
                txtOldObservations.Visibility = Visibility.Collapsed;
                observationLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtOldObservations.Visibility = Visibility.Visible;
                observationLabel.Visibility = Visibility.Visible;
            }
            foreach (Observation observation in ctx.Observations)
            {
                Observation Observations = new Observation();
                Observations.ID = observation.ID;
                Observations.Value = observation.Value;

                if (listOfObserations.Contains(observation.Value))
                    Observations.IsChecked = true;
                else
                    Observations.IsChecked = false;

                ObservatonCollection.Add(Observations);
            }
            return ObservatonCollection;
        }
    }
}

