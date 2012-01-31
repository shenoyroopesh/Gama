namespace RadiographyTracking
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using RadiographyTracking.LoginUI;
    using System;
    using MenuControl;

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Creates a new <see cref="MainPage"/> instance.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            loginStatus.MainPage = this;
        }

        /// <summary>
        /// After the Frame navigates, ensure the <see cref="HyperlinkButton"/> representing the current page is selected
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        /// <summary>
        /// If an error occurs during navigation, show an error window
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }

        private void ContentFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            Uri mappedUri = ContentFrame.UriMapper.MapUri(e.Uri);
            if (WebContext.Current.User.IsAuthenticated == false)
            {
                e.Cancel = true;
                LoginAndGoHome();
            }
        }

        /// <summary>
        /// This method forces the user to login if she has not already logged in
        /// </summary>
        /// <param name="uri"></param>
        public void LoginAndGoHome()
        {
            if (WebContext.Current.User.IsAuthenticated == false)
            {
                LoginRegistrationWindow loginRegistrationWindow = new LoginRegistrationWindow();
                loginRegistrationWindow.Closed +=
                    (s, e) =>
                        {
                            if(WebContext.Current.User.IsAuthenticated)
                            {
                                GoHome();
                            }
                        };
                loginRegistrationWindow.Show();
            }
        }

        public void GoHome()
        {
            ContentFrame.Navigate(new Uri("/Home", UriKind.Relative));
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.ContentFrame.Navigate(new Uri((sender as MenuItem).NavigationURI, UriKind.Relative));
        }
    }
}