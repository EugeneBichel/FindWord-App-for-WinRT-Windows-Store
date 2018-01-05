using Windows.UI.Xaml;

namespace FindWord.Messaging
{
    public class ShowProgressBarMessage:IMessage
    {
        public Visibility ProgressBarVisibility { get; private set; }

        public ShowProgressBarMessage(Visibility ProgressVisibility)
        {
            ProgressBarVisibility = ProgressVisibility;
        }
    }
}