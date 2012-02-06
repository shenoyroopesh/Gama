namespace RadiographyTracking
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using RadiographyTracking.LoginUI;
    using System;
    using MenuControl;
    using System.Linq;

    /// <summary>
    /// <see cref="UserControl"/> class providing the main UI for the application.
    /// </summary>
    public partial class MainPage : UserControl
    {
        //roles (lower letters)

        private const string Admin = "admin";
        private const string Clerk = "clerk";
        private const string Supervisor = "foundry supervisor";
        private const string ManagingDirector = "managing director";
        private const string Corrector = "corrector";


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
                                SetMenu();
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

        private void SetMenu()
        {
            string currentRole;

            try
            {
                currentRole = WebContext.Current.User.Roles.FirstOrDefault();
            } 
            catch
            {
                return;
            }

            switch(currentRole.ToLower())
            {
                case Admin:
                    adminMenuBar.Visibility = Visibility.Visible;
                    clerkMenuBar.Visibility = Visibility.Collapsed;
                    ManagingDirectorMenuBar.Visibility = Visibility.Collapsed;
                    supervisorMenuBar.Visibility = Visibility.Collapsed;
                    break;
                case ManagingDirector:
                    adminMenuBar.Visibility = Visibility.Collapsed;
                    clerkMenuBar.Visibility = Visibility.Collapsed;
                    ManagingDirectorMenuBar.Visibility = Visibility.Visible;
                    supervisorMenuBar.Visibility = Visibility.Collapsed;
                    break;
                case Supervisor:
                    adminMenuBar.Visibility = Visibility.Collapsed;
                    clerkMenuBar.Visibility = Visibility.Collapsed;
                    ManagingDirectorMenuBar.Visibility = Visibility.Collapsed;
                    supervisorMenuBar.Visibility = Visibility.Visible;
                    break;
                case Corrector:
                case Clerk:
                    adminMenuBar.Visibility = Visibility.Collapsed;
                    clerkMenuBar.Visibility = Visibility.Visible;
                    ManagingDirectorMenuBar.Visibility = Visibility.Collapsed;
                    supervisorMenuBar.Visibility = Visibility.Collapsed;
                    break;
                default:
                    adminMenuBar.Visibility = Visibility.Collapsed;
                    clerkMenuBar.Visibility = Visibility.Collapsed;
                    ManagingDirectorMenuBar.Visibility = Visibility.Collapsed;
                    supervisorMenuBar.Visibility = Visibility.Collapsed;
                    break;
            }
        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.ContentFrame.Navigate(new Uri((sender as MenuItem).NavigationURI, UriKind.Relative));
        }
    }
}