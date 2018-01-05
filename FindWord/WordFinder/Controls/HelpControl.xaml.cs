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
    public sealed partial class HelpControl : UserControl
    {
        public string Version { get; set; }

        public HelpControl()
        {
            this.InitializeComponent();
            BackButton.IsEnabled = true;

            Title = "Help";
            QuestionKeyword = "?";
            QuestionKeywordDescription = "The question mark indicates that there can be zero or one letter. For example, mar? matches: 'mar', 'mara', 'marb', 'mark', etc.";
            StarKeyword = "*";
            StarKeywordDescription="The asterisk indicates that there can be zero or more letters. For example, ab*c matches 'abc', 'abac', 'abaac', 'abaabc', etc.";
            PlusKeyword = "+";
            PlusKeywordDescription="The plus sign indicates that there can be one or more letters. For example, ab+c matches 'abac', 'ababc', and etc., but not 'ac'.";

            this.DataContext = this;
        }

        #region Public Properties

        public string Title { get; set; }
        public string QuestionKeyword { get; set; }
        public string QuestionKeywordDescription { get; set; }
        public string StarKeyword { get; set; }
        public string StarKeywordDescription { get; set; }
        public string PlusKeyword { get; set; }
        public string PlusKeywordDescription { get; set; }

        #endregion //Public Properties

        #region Public Methods

        public void Show()
        {
            VisualStateManager.GoToState(this, "HelpOpened", true);
        }

        public void Hide()
        {
            VisualStateManager.GoToState(this, "HelpClosed", true);
        }

        #endregion //Public Methods

        #region Event Handlers

        private void HelpLoaded(object sender, RoutedEventArgs e)
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
    }
}