using System.Collections.Generic;

namespace WordFinder.Messaging
{
    public class WordsResponseMessage : IMessage
    {
        public IList<string> Words { get; private set; }

        public WordsResponseMessage(IList<string> words = null)
        {
            Words = words;
        }
    }
}