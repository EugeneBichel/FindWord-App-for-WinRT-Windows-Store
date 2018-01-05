using System;
using System.Diagnostics;
using Windows.ApplicationModel.Search;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FindWord.ViewModels;

namespace FindWord.Views
{
    public sealed partial class Shell : UserControl
    {
        #region Fields

        private static ShellViewModel _model;

        #endregion //Fields

        #region Init

        public Shell()
        {
            InitializeComponent();
            Loaded += ShellLoaded;

            Window.Current.SizeChanged += this.WindowSizeChanged;

            RegisterSettings();
        }

        private void ShellLoaded(object sender, RoutedEventArgs e)
        {
            _model = (ShellViewModel)DataContext;

            DoUpdateLayout();
        }

        #endregion //Init

        #region Public Properties

        public Frame Frame
        {
            get { return NavigationFrame; }
        }

        #endregion //Public Properties

        #region Update ViewState

        private void DoUpdateLayout()
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped)
                UpdateLayout(true);
            else
                UpdateLayout(false);
        }

        private void WindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            DoUpdateLayout();
        }

        private void UpdateLayout(bool isSnappedOrShowApplMessage)
        {
            if (isSnappedOrShowApplMessage)
            {
                if (grdHeader != null)
                    grdHeader.Height = 90;

                BackButton.Style = (Style)App.Current.Resources["SnappedBackButtonStyle"];
                Title.Style = (Style)App.Current.Resources["SnappedPageTitleStyle"];
                grdFrame.Style = (Style)App.Current.Resources["SnappedFrameGridStyle"];
            }
            else
            {
                if (grdHeader != null)
                    grdHeader.Height = 100;

                BackButton.Style = (Style)App.Current.Resources["UnsnappedBackButtonStyle"];
                Title.Style = (Style)App.Current.Resources["PageTitleStyle"];
                grdFrame.Style = (Style)App.Current.Resources["FrameGridStyle"];
            }
        }

        #endregion //Update ViewState

        #region Settings Pane

        private void RegisterSettings()
        {
            SettingsPane.GetForCurrentView().CommandsRequested += AppCommandsRequested;
        }

        private void AppCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args.Request.ApplicationCommands.Count == 0)
            {
                var aboutCommand = new SettingsCommand("About", "About", new UICommandInvokedHandler(OnAboutSettingsCommand));
                args.Request.ApplicationCommands.Add(aboutCommand);

                var privacyCommand = new SettingsCommand("Privacy", "Privacy", new UICommandInvokedHandler(OnPrivacySettingsCommand));
                args.Request.ApplicationCommands.Add(privacyCommand);

                var helpCommand = new SettingsCommand("Help", "Help", new UICommandInvokedHandler(OnHelpSettingsCommand));
                args.Request.ApplicationCommands.Add(helpCommand);
            }
        }

        private void OnAboutSettingsCommand(IUICommand command)
        {
            SettingsCommand settingsCommand = (SettingsCommand)command;
            if (settingsCommand.Label == "About")
                this.AboutPage.Show();
        }

        private void OnPrivacySettingsCommand(IUICommand command)
        {
            SettingsCommand settingsCommand = (SettingsCommand)command;
            if (settingsCommand.Label == "Privacy")
                this.PrivacyPage.Show();
        }

        private void OnHelpSettingsCommand(IUICommand command)
        {
            SettingsCommand settingsCommand = (SettingsCommand)command;
            if (settingsCommand.Label == "Help")
                this.HelpPage.Show();
        }

        #endregion //Settings Pane
    }
}