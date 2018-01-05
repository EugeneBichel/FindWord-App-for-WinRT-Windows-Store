using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FindWord.Controls
{
    public sealed partial class PrivacyControl : UserControl
    {
        public string Version { get; set; }

        public PrivacyControl()
        {
            this.InitializeComponent();
            BackButton.IsEnabled = true;

            Policy = "This application does not collect or transmit any user's personal information. No personal information is used, stored. If you would like to report violations of this policy, please contact ";
            Email = "support@aemobileapps.com";
            Title = "Privacy policy";

            this.DataContext = this;
        }

        #region Public Properties

        public string Email { get; set; }

        public string Policy { get; set; }

        public string Title { get; set; }

        #endregion //Public Properties

        #region Event Handlers

        private void PrivacyLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void OverlayTapped(object sender, TappedRoutedEventArgs e)
        {
            Hide();
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            Hide();
            SettingsPane.Show();
        }

        private async void BtnSendEmailTapped(object sender, TappedRoutedEventArgs e)
        {
            var mailto = new Uri("mailto:?to=bichel.eugen@gmail.com");
            await Windows.System.Launcher.LaunchUriAsync(mailto);
        }

        #endregion //Event Handlers

        #region Public Methods

        public void Show()
        {
            VisualStateManager.GoToState(this, "PrivacyOpened", true);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "PrivacyClosed", true);
        }

        #endregion //Public Methods
    }
}