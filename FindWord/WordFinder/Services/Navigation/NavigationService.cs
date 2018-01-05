using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FindWord.Common;
using FindWord.Messaging;
using FindWord.Views;

namespace FindWord.Services
{
    public class NavigationService : INavigationService
    {
        #region Fields

        public event NavigatingCancelEventHandler Navigating = delegate { };
        public event Action<bool> CanGoBackNavigating;

        private static Frame _frame;
        private readonly IHub _messageHub;
        private AppState _appState;

        #endregion //Fields

        #region Init Methods

        public NavigationService(IHub messageHub, AppState appState)
        {
            GoBackCommand = new DelegateCommand(GoBack);
            _messageHub = messageHub;
            _appState = appState;
            _navigationStack = new Stack<ViewType>();
        }

        public void InitializeFrame(Frame frame)
        {
            if (_frame != null)
                _frame.Navigating -= FrameNavigating;

            _frame = frame;
            _frame.Navigating += FrameNavigating;
        }

        #endregion //Init Methods

        #region Public Properties

        private ViewType _currentView;
        public ViewType CurrentView
        {
            get { return _currentView; }
            private set
            {
                _currentView = value;
                _appState.SessionState.CurrentView = _currentView;
            }
        }

        private Stack<ViewType> _navigationStack;
        public Stack<ViewType> NavigationStack
        {
            get { return _navigationStack; }
            private set
            {
                _navigationStack = value;
            }
        }

        #endregion //Public Properties

        #region GoBackCommand

        public ICommand GoBackCommand { get; set; }

        private async void GoBack()
        {
            if (_frame.CanGoBack)
            {
                if (NavigationStack != null && NavigationStack.Count >= 2)
                {
                    //pop current view
                    NavigationStack.Pop();
                    //peek previous view
                    CurrentView = NavigationStack.Peek();

                    _appState.SessionState.NavigationStack = NavigationStack;
                }
                else
                    CurrentView = ViewType.StartPage;

                await Navigate();
            }
            else
                OnCanGoBackNavigating(false);

            _appState.SessionState.NavigationState = _frame.GetNavigationState();
        }

        #endregion //GoBackCommand

        #region Navigation Methods

        public void Navigate(Type source, object parameter = null)
        {
            if (_frame == null)
                throw new InvalidOperationException("Frame has not been initialized.");

            _frame.Navigate(source, parameter);

            if (source != typeof(Shell))
            {
                if (source == typeof(StartPage))
                    CurrentView = ViewType.StartPage;
                else if (source == typeof(WordsWithTheSameLengthPage))
                    CurrentView = ViewType.WordsWithTheSameLengthPage;

                if (NavigationStack.Count == 0 || NavigationStack.FirstOrDefault() != CurrentView)
                {
                    NavigationStack.Push(CurrentView);

                    _appState.SessionState.NavigationStack = NavigationStack;
                }
                if (NavigationStack.Count > 1)
                    OnCanGoBackNavigating(true);

                _appState.SessionState.NavigationState = _frame.GetNavigationState();
            }

            ((DelegateCommandBase)GoBackCommand).RaiseCanExecuteChanged();
        }

        public async void Refresh()
        {
            await Navigate();
        }

        public async Task LoadSavedSession()
        {
            if (_appState == null || _appState.SessionState == null)
                return;

            if (_appState.SessionState.NavigationState != null)
                _frame.SetNavigationState(_appState.SessionState.NavigationState);
            if (_appState.SessionState.NavigationStack != null)
                _navigationStack = _appState.SessionState.NavigationStack;
            if (_appState.SessionState.CurrentView != null)
                CurrentView = _appState.SessionState.CurrentView;

            if (CurrentView == ViewType.WordsWithTheSameLengthPage)
                await _messageHub.Send<ShowWordsWithTheSameLengthPageMessage>(new ShowWordsWithTheSameLengthPageMessage());
            else if (CurrentView == ViewType.StartPage)
                await _messageHub.Send<ShowStartPageMessage>(new ShowStartPageMessage());
        }

        public void ShowProgressBar(Visibility progressBarVisibility)
        {
            _messageHub.Send<ShowProgressBarMessage>(new ShowProgressBarMessage(progressBarVisibility));
        }

        public void ShowProgressRing(Visibility progressRingVisibility)
        {
            _messageHub.Send<ShowProgressRingMessage>(new ShowProgressRingMessage(progressRingVisibility));
        }

        #endregion //Navigation Methods

        #region Help Methods

        private void FrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            Navigating(sender, e);
        }

        private void OnCanGoBackNavigating(bool canGoBack)
        {
            var handler = CanGoBackNavigating;
            if (handler != null)
                handler(canGoBack);
        }

        private async Task Navigate()
        {
            if (_frame == null)
                throw new InvalidOperationException("Frame has not been initialized.");

            if (NavigationStack.Count <= 1)
                OnCanGoBackNavigating(false);
            else
                OnCanGoBackNavigating(true);

            if (CurrentView == ViewType.WordsWithTheSameLengthPage)
                await _messageHub.Send<ShowWordsWithTheSameLengthPageMessage>(new ShowWordsWithTheSameLengthPageMessage(string.Format("{0} LETTERS", _appState.SessionState.Length), _appState.SessionState.Pattern));
            else if (CurrentView == ViewType.StartPage)
                await _messageHub.Send<ShowStartPageMessage>(new ShowStartPageMessage());
        }

        #endregion //Help Methods
    }
}