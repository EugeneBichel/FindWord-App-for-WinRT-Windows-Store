using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using FindWord.Common;
using FindWord.Messaging;
using FindWord.Services;
using System;

namespace FindWord.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private const int LengthOfMaxWord = 100;

        public event EventHandler LettesChanged = delegate { };

        #region Private Fields

        private Dictionary<int, List<string>> _findedWordsWithLength;
        private readonly INavigationService _navigator;
        private readonly IHub _messageHub;
        private readonly IDialogService _dialogService;
        private readonly SearchService _searchService;
        private readonly UndoRedoService _undoRedoService;
        private string _pattern;

        #endregion //Private Fields

        #region Constructor

        public StartPageViewModel(INavigationService navigator,
            IHub messageHub,
            IDialogService dialogService,
            SearchService searchService,
            UndoRedoService undoRedoService)
        {
            _navigator = navigator;
            _messageHub = messageHub;
            _dialogService = dialogService;
            _searchService = searchService;
            _undoRedoService = undoRedoService;

            TopN = 10;

            UndoCommand = _undoRedoService.UndoCommand;
            RedoCommand = _undoRedoService.RedoCommand;
            UndoEnabled = _undoRedoService.UndoEnabled;
            RedoEnabled = _undoRedoService.RedoEnabled;

            _undoRedoService.UndoEvent += UndoRedoServiceUndoEvent;
            _undoRedoService.RedoEvent += UndoRedoServiceRedoEvent;
        }

        #endregion //Constructor

        #region Public Properties

        private ObservableCollection<string> _letters;
        public ObservableCollection<string> Letters
        {
            get
            {
                if (_letters == null)
                    _letters = new ObservableCollection<string>();
                return _letters;
            }
            set
            {
                SetProperty(ref _letters, value);
            }
        }

        public int TopN { get; set; }

        private ObservableCollection<DataGroup> _allGroups;
        public ObservableCollection<DataGroup> AllGroups
        {
            get
            {
                if (_allGroups == null)
                    _allGroups = new ObservableCollection<DataGroup>();

                return _allGroups;
            }
            set
            {
                _allGroups = value;
            }
        }
        
        private bool _undoEnabled;
        public bool UndoEnabled
        {
            get
            {
                return _undoEnabled;
            }
            set { SetProperty<bool>(ref _undoEnabled, value); }
        }

        private bool _redoEnabled;
        public bool RedoEnabled
        {
            get
            {
                return _redoEnabled;
            }
            set { SetProperty<bool>(ref _redoEnabled, value); }
        }

        #endregion //Public Properties

        #region Commands

        public ICommand UndoCommand { get; set; }

        public ICommand RedoCommand { get; set; }

        #endregion //Commands

        #region Public Methods

        public async Task FindMatchedWords()
        {
            _pattern = _searchService.GetSearchPattern(Letters);

            _findedWordsWithLength = await _searchService.FindWordsByPatternAsync(_pattern, TopN);

            InitAppGroupsProp();
        }

        public void ShowWordsWithTheSameLengthView(string title)
        {
            _messageHub.Send<ShowWordsWithTheSameLengthPageMessage>(new ShowWordsWithTheSameLengthPageMessage(title, _pattern, TopN));
        }

        public void ShowProgressBar(Visibility progressBarVisibility)
        {
            _messageHub.Send<ShowProgressBarMessage>(new ShowProgressBarMessage(progressBarVisibility));
        }

        public void SaveCurrentLettersState(ObservableCollection<string> _letters, bool isUser)
        {
            if (_undoRedoService == null)
                return;

            _undoRedoService.SaveCurrentLettersState(_letters, isUser);

            UndoEnabled = _undoRedoService.UndoEnabled;
            RedoEnabled = _undoRedoService.RedoEnabled;
        }

        #endregion //Public Methods

        #region Private Methods

        private bool InitAppGroupsProp()
        {
            var allGroups = new ObservableCollection<DataGroup>();

            if (_findedWordsWithLength == null)
                return false;

            for (var i = 0; i < _findedWordsWithLength.Count; i++)
            {
                if (_findedWordsWithLength.Values.ElementAt(i).Count == 0)
                    continue;

                var grWords = new DataGroup();
                grWords.Title = string.Format("{0} LETTERS", _findedWordsWithLength.Keys.ElementAt(i));
                grWords.Items = new ObservableCollection<object>(_findedWordsWithLength.Values.ElementAt(i));
                allGroups.Add(grWords);
            }

            AllGroups = new ObservableCollection<DataGroup>(allGroups);

            return true;
        }

        #endregion //Private Methods

        #region Undo/Redo Event Handlers

        private void UndoRedoServiceRedoEvent(object sender, ObservableCollection<string> e)
        {
            Letters = e;

            UndoEnabled = _undoRedoService.UndoEnabled;
            RedoEnabled = _undoRedoService.RedoEnabled;

            LettesChanged(this, new EventArgs());
        }

        private void UndoRedoServiceUndoEvent(object sender, ObservableCollection<string> e)
        {
            Letters = e;

            UndoEnabled = _undoRedoService.UndoEnabled;
            RedoEnabled = _undoRedoService.RedoEnabled;

            LettesChanged(this, new EventArgs());
        }

        #endregion //Undo/Redo Event Handlers
    }
}