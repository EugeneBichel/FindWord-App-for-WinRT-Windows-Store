using System.Collections.Generic;

namespace WordFinder.Messaging
{
    public class WordsRequestMessage : IMessage
    {
        public IList<string> Words { get; private set; }

        public WordsRequestMessage(IList<string> words = null)
        {
            Words = words;
        }
    }
}