using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using FindWord.Common;
using FindWord.Messaging;
using FindWord.Services;

namespace FindWord.ViewModels
{
    public sealed class ShellViewModel : BindableBase
    {
        #region Fields

        private readonly INavigationService _navigator;
        private readonly IHub _messageHub;
        private AppState _appState;
        private readonly ResourceLoader _resourceLoader;

        #endregion //Fields

        #region Constructor

        public ShellViewModel(INavigationService navigator,
            IHub messageHub,
            AppState appState)
        {
            _navigator = navigator;
            _messageHub = messageHub;
            _appState = appState;

            _resourceLoader = new ResourceLoader();
            BackCommand = _navigator.GoBackCommand;

            ProgressBarVisibility = Visibility.Collapsed;
            ProgressRingVisibility = Visibility.Collapsed;

            _navigator.CanGoBackNavigating += NavigatorCanGoBackNavigating;
        }

        #endregion //Constructor

        #region Public Properties

        private Visibility _progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get { return _progressBarVisibility; }
            set
            {
                SetProperty<Visibility>(ref _progressBarVisibility, value);
            }
        }

        private Visibility _progressRingVisibility;
        public Visibility ProgressRingVisibility
        {
            get { return _progressRingVisibility; }
            set
            {
                SetProperty<Visibility>(ref _progressRingVisibility, value);
            }
        }

        private bool _backEnabled;
        public bool BackEnabled
        {
            get
            {
                return _backEnabled;
            }
            set { SetProperty<bool>(ref _backEnabled, value); }
        }

        #endregion //Public Properties

        #region Commands

        public ICommand BackCommand { get; set; }

        #endregion //Commands

        #region Navigation Event Handlers

        private void NavigatorCanGoBackNavigating(bool canGoBack)
        {
            BackEnabled = canGoBack;
        }

        #endregion //Navigation Event Handlers
    }
}