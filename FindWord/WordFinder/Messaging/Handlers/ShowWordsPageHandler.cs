using Windows.UI.Xaml;
using WordFinder.Services;
using WordFinder.Views;

namespace WordFinder.Messaging
{
    public class ShowWordsPageHandler : IHandler<ShowWordsPageMessage>
    {
        private readonly IHub _messageHub;
        private readonly INavigationService _navigator;
        private readonly AppState _appState;

        public ShowWordsPageHandler(IHub messageHub,
            INavigationService navigator,
            AppState appState)
        {
            _messageHub = messageHub;
            _navigator = navigator;
            _appState = appState;
        }

        public async void Handle(ShowWordsPageMessage message)
        {
            _navigator.ShowProgressRing(Visibility.Visible);
            _navigator.ShowProgressBar(Visibility.Visible);

            await _messageHub.Send<WordsRequestMessage>(new WordsRequestMessage(message.Words));

            _navigator.ShowProgressRing(Visibility.Collapsed);
            _navigator.ShowProgressBar(Visibility.Collapsed);

            _navigator.Navigate(typeof(WordsPage));
        }
    }
}