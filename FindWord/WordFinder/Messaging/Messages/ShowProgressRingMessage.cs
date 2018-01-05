using Windows.UI.Xaml;

namespace FindWord.Messaging
{
    public class ShowProgressRingMessage : IMessage
    {
        public Visibility ProgressVisibility { get; private set; }

        public ShowProgressRingMessage(Visibility progressVisibility)
        {
            ProgressVisibility = progressVisibility;
        }
    }
}