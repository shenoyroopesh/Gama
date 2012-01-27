namespace RadiologyTracking.LoginUI
{
    using System.ComponentModel;
    using System.Globalization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="UserControl"/> class that shows the current login status and allows login and logout.
    /// </summary>
    public partial class LoginStatus : UserControl
    {
        MainPage mainPage;

        /// <summary>
        /// Creates a new <see cref="LoginStatus"/> instance.
        /// </summary>
        public LoginStatus()
        {
            this.InitializeComponent();

            if (DesignerProperties.IsInDesignTool)
            {
                VisualStateManager.GoToState(this, "loggedOut", false);
            }
            else
            {
                this.DataContext = WebContext.Current;
                WebContext.Current.Authentication.LoggedIn += this.Authentication_LoggedIn;
                WebContext.Current.Authentication.LoggedOut += this.Authentication_LoggedOut;
                this.UpdateLoginState();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginRegistrationWindow loginWindow = new LoginRegistrationWindow();
            if (mainPage != null)
            {
                mainPage.LoginAndGoHome();
            }
            else
            {
                loginWindow.Show();
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            WebContext.Current.Authentication.Logout(logoutOperation =>
            {
                if (logoutOperation.HasError)
                {
                    ErrorWindow.CreateNew(logoutOperation.Error);
                    logoutOperation.MarkErrorAsHandled();
                }
                else
                {
                    //force login again
                    LoginButton_Click(sender, e);
                }
            }, /* userState */ null);
        }

        private void Authentication_LoggedIn(object sender, AuthenticationEventArgs e)
        {
            this.UpdateLoginState();
        }

        private void Authentication_LoggedOut(object sender, AuthenticationEventArgs e)
        {
            this.UpdateLoginState();
        }

        private void UpdateLoginState()
        {
            if (WebContext.Current.User.IsAuthenticated)
            {
                this.welcomeText.Text = string.Format(
                    CultureInfo.CurrentUICulture,
                    ApplicationStrings.WelcomeMessage,
                    WebContext.Current.User.DisplayName);
            }
            else
            {
                this.welcomeText.Text = ApplicationStrings.AuthenticatingMessage;
            }

            if (WebContext.Current.Authentication is WindowsAuthentication)
            {
                VisualStateManager.GoToState(this, "windowsAuth", true);
            }
            else
            {
                VisualStateManager.GoToState(this, (WebContext.Current.User.IsAuthenticated) ? "loggedIn" : "loggedOut", true);
            }
        }

        private void passwordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow window = new ChangePasswordWindow();
            window.Show();
        }
    }
}
