using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using FindWord.Common;
using System;

namespace FindWord.Services
{
    public class UndoRedoService
    {
        public event EventHandler<ObservableCollection<string>> UndoEvent = delegate { };
        public event EventHandler<ObservableCollection<string>> RedoEvent = delegate { };

        private Stack<ObservableCollection<string>> _undoStack;
        private Stack<ObservableCollection<string>> _redoStack;

        public UndoRedoService()
        {
            _undoStack = new Stack<ObservableCollection<string>>();
            //_undoStack.Push(new ObservableCollection<string> { "*" });
            _redoStack = new Stack<ObservableCollection<string>>();

            UndoEnabled = false;
            RedoEnabled = false;

            UndoCommand = new DelegateCommand(Undo);
            RedoCommand = new DelegateCommand(Redo);
        }

        #region Public Properties

        public bool UndoEnabled { get; set; }

        public bool RedoEnabled { get; set; }

        public ObservableCollection<string> CurrentLettersState { get; set; }

        #endregion //Public Properties

        #region Undo/Redo Commands

        public ICommand UndoCommand { get; set; }

        private static bool _isFirstUndo = true;
        private static bool _isFirstRedo = true;

        private void Undo()
        {
            if (_undoStack.Count == 0)
            {
                UndoEnabled = false;
                _isFirstUndo = true;
                UndoEvent(this, CurrentLettersState);
                return;
            }

            if (_isFirstUndo)
            {
                CurrentLettersState = _undoStack.Pop();
                _redoStack.Push(CurrentLettersState);
                RedoEnabled = true;
                _isFirstUndo = false;
            }

            if (_undoStack.Count == 0)
            {
                UndoEnabled = false;
                _isFirstUndo = true;
                UndoEvent(this, CurrentLettersState);
                return;
            }

            CurrentLettersState = _undoStack.Pop();

            if (_undoStack.Count == 0)
            {
                UndoEnabled = false;
                _isFirstUndo = true;
            }

            _redoStack.Push(CurrentLettersState);
            RedoEnabled = true;

            UndoEvent(this, CurrentLettersState);
        }

        public ICommand RedoCommand { get; set; }

        private void Redo()
        {
            if (_redoStack.Count == 0)
            {
                RedoEnabled = false;
                RedoEvent(this, CurrentLettersState);
                return;
            }

            if (_isFirstRedo)
            {
                CurrentLettersState = _redoStack.Pop();
                _undoStack.Push(CurrentLettersState);
                UndoEnabled = true;
                _isFirstRedo = false;
            }

            if (_redoStack.Count == 0)
            {
                RedoEnabled = false;
                _isFirstRedo = true;
                RedoEvent(this, CurrentLettersState);
                return;
            }

            CurrentLettersState = _redoStack.Pop();

            if (_redoStack.Count == 0)
            {
                RedoEnabled = false;
                _isFirstRedo = true;
            }

            _undoStack.Push(CurrentLettersState);
            UndoEnabled = true;

            RedoEvent(this, CurrentLettersState);
        }

        #endregion //Undo/Redo Commands

        #region Public Methods

        public void SaveCurrentLettersState(ObservableCollection<string> letters, bool isUser)
        {
            var lettersState = new ObservableCollection<string>(letters);

            _undoStack.Push(lettersState);
            if (isUser)
                UndoEnabled = true;
        }

        #endregion //Public Methods
    }
}