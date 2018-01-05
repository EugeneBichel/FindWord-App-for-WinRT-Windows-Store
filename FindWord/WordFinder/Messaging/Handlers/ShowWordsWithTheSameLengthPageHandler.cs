using FindWord.Services;
using FindWord.ViewModels;
using FindWord.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FindWord.Messaging
{
    public class ShowWordsWithTheSameLengthPageHandler : IHandler<ShowWordsWithTheSameLengthPageMessage>
    {
        private readonly INavigationService _navigator;
        private WordsWithTheSameLengthViewModel _model;

        public ShowWordsWithTheSameLengthPageHandler(INavigationService navigator,WordsWithTheSameLengthViewModel model)
        {
            _navigator = navigator;
            _model = model;
        }

        public async void Handle(ShowWordsWithTheSameLengthPageMessage message)
        {
            _model.Title = message.Title;
            _model.SelectedTopN = message.TopN;
            _model.Pattern = message.Pattern;

            _navigator.Navigate(typeof(WordsWithTheSameLengthPage));
        }
    }
}