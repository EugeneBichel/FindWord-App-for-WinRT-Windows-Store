using System.Collections.Generic;

namespace WordFinder.Messaging
{
    public class ShowWordsPageMessage : IMessage
    {
        public IList<string> Words { get; private set; }

        public ShowWordsPageMessage(IList<string> words = null)
        {
            Words = words;
        }
    }
}