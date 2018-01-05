using FindWord.ViewModels;
namespace FindWord.Messaging
{
    public class ShowProgressBarHandler : IHandler<ShowProgressBarMessage>
    {
        private ShellViewModel _shellViewModel;

        public ShowProgressBarHandler(ShellViewModel shellViewModel)
        {
            _shellViewModel = shellViewModel;
        }

        public void Handle(ShowProgressBarMessage message)
        {
            _shellViewModel.ProgressBarVisibility = message.ProgressBarVisibility;
        }
    }
}