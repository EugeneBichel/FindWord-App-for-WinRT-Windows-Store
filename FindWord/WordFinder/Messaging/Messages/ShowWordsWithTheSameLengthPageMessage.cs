using System.Collections.Generic;

namespace FindWord.Messaging
{
    public class ShowWordsWithTheSameLengthPageMessage : IMessage
    {
        public string Pattern { get; private set; }
        public string Title { get; private set; }
        public int TopN { get; set; }

        public ShowWordsWithTheSameLengthPageMessage(string title = "", string pattern="",int topN=0)
        {
            Title = title;
            Pattern = pattern;
            TopN = topN;
        }
    }
}