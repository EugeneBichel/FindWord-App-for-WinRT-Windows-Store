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
    public sealed partial class AboutControl : UserControl
    {
        public string Version { get; set; }

        public AboutControl()
        {
            this.InitializeComponent();
            BackButton.IsEnabled = true;

            PackageVersion version = Package.Current.Id.Version;
            Version = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);

            Title = "About";
            VersionTitle = "Version";
            OurName = "aemobileapps";
            ProductDetailsTitle = "Product Details";
            ProductDetails = "This application allows to find words by setting some letters";

            this.DataContext = this;
        }

        #region Public Properties

        public string Title { get; set; }
        public string VersionTitle { get; set; }
        public string OurName { get; set; }
        public string ProductDetailsTitle { get; set; }
        public string ProductDetails { get; set; }

        #endregion //Public Properties

        #region Event Handlers

        private void AboutLoaded(object sender, RoutedEventArgs e)
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

        #endregion //Event Handlers

        #region Public Methods

        public void Show()
        {
            VisualStateManager.GoToState(this, "AboutOpened", true);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "AboutClosed", true);
        }

        #endregion //Public Methods
    }
}