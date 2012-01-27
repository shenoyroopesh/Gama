namespace RadiologyTracking
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using RadiologyTracking.LoginUI;
    using System;

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
        }

        /// <summary>
        /// After the Frame navigates, ensure the <see cref="HyperlinkButton"/> representing the current page is selected
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
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
                                // Try again!
                                ContentFrame.Navigate(new Uri("/Home", UriKind.Relative));
                            }
                        };
                loginRegistrationWindow.Show();
            }
        }
    }
}