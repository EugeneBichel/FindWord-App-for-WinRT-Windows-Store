using System;
using System.Collections.Generic;

namespace FindWord.Services
{
    public sealed class SessionState
    {
        public string SearchWord { get; set; }
        public string Pattern { get; set; }
        public int Length { get; set; }
        public ViewType CurrentView { get; set; }
        public string NavigationState { get; set; }
        public Stack<ViewType> NavigationStack { get; set; }

        public bool IsValid()
        {
            return (CurrentView == ViewType.StartPage
                    || (CurrentView == ViewType.WordsWithTheSameLengthPage && !String.IsNullOrEmpty(SearchWord)));
        }
    }
}