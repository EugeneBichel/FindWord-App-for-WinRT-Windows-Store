using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using FindWord.Common;
using FindWord.Messaging;
using FindWord.Services;

namespace FindWord.ViewModels
{
    public class WordsWithTheSameLengthViewModel : BindableBase
    {
        private readonly IHub _messageHub;
        private readonly SearchService _searchService;

        #region Constructor

        public WordsWithTheSameLengthViewModel(IHub messageHub, SearchService searchService)
        {
            _messageHub = messageHub;
            _searchService = searchService;
        }

        #endregion //Constructor

        #region Public Properties

        public string Title { get; set; }

        public string Pattern { get; set; }

        private ObservableCollection<string> _words;
        public ObservableCollection<string> Words 
        {
            get
            {
                return _words;
            }
            set
            {
                _words = value;
            }
        }

        private int _selectedTopN;
        public int SelectedTopN 
        {
            get 
            {
                if (_selectedTopN <= 0)
                    _selectedTopN = 10;
                return _selectedTopN; 
            }
            set
            {
                SetProperty(ref _selectedTopN, value);
            }
        }

        #endregion //Public Properties

        #region Public Methods

        private int key = 1;

        public async Task ShowSelectedNumberOfWordsAsync()
        {
            if (Title == null)
                return;

            try
            {
                ShowProgressBar(Windows.UI.Xaml.Visibility.Visible);

                var length = Title.Split(' ')[0];

                key = int.Parse(length);

                var findedWordsWithLength = await _searchService.FindWordsByPatternAsync(Pattern, SelectedTopN);

                Words = new ObservableCollection<string>(findedWordsWithLength[key]);
               
                ShowProgressBar(Windows.UI.Xaml.Visibility.Collapsed);
            }
            catch(Exception)
            {
                Words = new ObservableCollection<string>();
            }
        }

        public void ShowProgressBar(Visibility progressBarVisibility)
        {
            _messageHub.Send<ShowProgressBarMessage>(new ShowProgressBarMessage(progressBarVisibility));
        }

        public async void Refresh()
        {
            await _messageHub.Send<ShowWordsWithTheSameLengthPageMessage>(new ShowWordsWithTheSameLengthPageMessage(Title, Pattern, SelectedTopN));
        }

        #endregion //Public Methods
    }
}