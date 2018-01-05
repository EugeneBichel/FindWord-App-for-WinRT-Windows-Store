using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using FindWord.Common;
using FindWord.ViewModels;

namespace FindWord.Views
{
    public sealed partial class WordsWithTheSameLengthPage : LayoutAwarePage
    {    
        #region Fields

        private WordsWithTheSameLengthViewModel _model;

        #endregion //Fields

        #region Init Page

        public WordsWithTheSameLengthPage()
        {
            this.InitializeComponent();
            this.SizeChanged += OnPageSizeChanged;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.DataContext is WordsWithTheSameLengthViewModel)
            {
                _model = (WordsWithTheSameLengthViewModel)this.DataContext;

                if (_model.SelectedTopN == 10)
                {
                    RadioButtonTenWords.Checked -= TenRadioButtonChecked;
                    RadioButtonTenWords.IsChecked = true;
                    RadioButtonTenWords.Checked += TenRadioButtonChecked;
                }
                else if (_model.SelectedTopN == 30)
                {
                    RadioButtonThirdWords.Checked -= ThirdRadioButtonChecked;
                    RadioButtonThirdWords.IsChecked = true;
                    RadioButtonThirdWords.Checked += ThirdRadioButtonChecked;
                }
                else if (_model.SelectedTopN == 50)
                {
                    RadioButtonFiftyWords.Checked -= FiftyRadioButtonChecked;
                    RadioButtonFiftyWords.IsChecked = true;
                    RadioButtonFiftyWords.Checked += FiftyRadioButtonChecked;
                }
                else if (_model.SelectedTopN == 100)
                {
                    RadioButtonHundredWords.Checked -= HundredRadioButtonChecked;
                    RadioButtonHundredWords.IsChecked = true;
                    RadioButtonHundredWords.Checked += HundredRadioButtonChecked;
                }
                else if (_model.SelectedTopN == 200)
                {
                    RadioButtonTwoHundredWords.Checked -= TwoHundredRadioButtonChecked;
                    RadioButtonTwoHundredWords.IsChecked = true;
                    RadioButtonTwoHundredWords.Checked += TwoHundredRadioButtonChecked;
                }

                await _model.ShowSelectedNumberOfWordsAsync();

                if (ScrollViewerWordsFilledView.Visibility == Visibility.Visible)
                    lstWordsFilledView.ItemsSource = _model.Words;
                else if (ScrollViewerWordsFullScreenPortraitView.Visibility == Visibility.Visible)
                    lstWordsPortraitView.ItemsSource = _model.Words;
                else if (ScrollViewerWordsSnappedView.Visibility == Visibility.Visible)
                    lstWordsSnappedView.ItemsSource = _model.Words;

                _model.ShowProgressBar(Visibility.Collapsed);
            }
        }

        #endregion //Init Page

        #region Change ViewSate

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetStylesForSelectedAppView();
        }

        private void SetStylesForSelectedAppView()
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Filled)
            {
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFilledView"];
                txtBlockTopN.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFilledView"];
                RadioButtonTenWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonThirdWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonFiftyWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonHundredWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonTwoHundredWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
            {
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFilledView"];
                txtBlockTopN.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFilledView"];
                RadioButtonTenWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonThirdWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonFiftyWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonHundredWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonTwoHundredWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped)
            {
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleSnappedView"];
                txtBlockTopN.Style = (Style)App.Current.Resources["HeaderTextBlockStyleSnappedView"];
                txtBlockTopN.Margin = new Thickness(5, 0, 5, 0);

                RadioButtonTenWords.Style = (Style)App.Current.Resources["RadioButtonSnappedView"];
                RadioButtonThirdWords.Style = (Style)App.Current.Resources["RadioButtonSnappedView"];
                RadioButtonFiftyWords.Style = (Style)App.Current.Resources["RadioButtonSnappedView"];
                RadioButtonHundredWords.Style = (Style)App.Current.Resources["RadioButtonSnappedView"];
                RadioButtonTwoHundredWords.Style = (Style)App.Current.Resources["RadioButtonSnappedView"];
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
            {
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFullScreenPortraitView"];
                txtBlockTopN.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFullScreenPortraitView"];
                RadioButtonTenWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonThirdWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonFiftyWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonHundredWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
                RadioButtonTwoHundredWords.Style = (Style)App.Current.Resources["RadioButtonFilledView"];
            }

            if (ScrollViewerWordsFilledView.Visibility == Visibility.Visible)
                lstWordsFilledView.ItemsSource = _model.Words;
            else if (ScrollViewerWordsFullScreenPortraitView.Visibility == Visibility.Visible)
                lstWordsPortraitView.ItemsSource = _model.Words;
            else if (ScrollViewerWordsSnappedView.Visibility == Visibility.Visible)
                lstWordsSnappedView.ItemsSource = _model.Words;
        }

        #endregion //Change ViewSate

        private void TenRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (_model == null)
                return;

            _model.SelectedTopN = 10;

            this.DataContext = _model;

            _model.Refresh();
        }

        private void ThirdRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (_model == null)
                return;

            _model.SelectedTopN = 30;

            this.DataContext = _model;

            _model.Refresh();
        }

        private void FiftyRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (_model == null)
                return;

            _model.SelectedTopN = 50;

            this.DataContext = _model;

            _model.Refresh();
        }

        private void HundredRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (_model == null)
                return;

            _model.SelectedTopN = 100;

            this.DataContext = _model;

            _model.Refresh();
        }

        private void TwoHundredRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (_model == null)
                return;

            _model.SelectedTopN = 200;

            this.DataContext = _model;

            _model.Refresh();
        }
    }
}