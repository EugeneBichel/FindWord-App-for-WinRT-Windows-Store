using FindWord.Services;
using FindWord.Views;

namespace FindWord.Messaging
{
    public class ShowStartPageHandler : IHandler<ShowStartPageMessage>
    {
        private readonly IHub _messageHub;
        private readonly INavigationService _navigator;

        public ShowStartPageHandler(IHub messageHub,
            INavigationService navigator)
        {
            _messageHub = messageHub;
            _navigator = navigator;
        }

        public async void Handle(ShowStartPageMessage message)
        {
            _navigator.Navigate(typeof(StartPage));
        }
    }
}