using FindWord.ViewModels;
namespace FindWord.Messaging
{
    public class ShowProgressRingHandler : IHandler<ShowProgressRingMessage>
    {
        private ShellViewModel _shellViewModel;

        public ShowProgressRingHandler(ShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
        }

        public void Handle(ShowProgressRingMessage message)
        {
            _shellViewModel.ProgressRingVisibility = message.ProgressVisibility;
        }
    }
}