using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FindWord.Common;
using FindWord.ViewModels;

namespace FindWord.Views
{
    public sealed partial class StartPage : LayoutAwarePage
    {    
        #region Fields

        //2 buttons+popup  plus charContainers
        private const int StartPosCharContainersInCanvas = 3;

        private StartPageViewModel _model;

        private double _btnLeftX;
        private double _btnRightX;

        private double _charContainerX;
        private double _charContainerY;
        private double _defCharContainerX;
        private double _defCharContainerY;

        private double _horMiddlePoint;

        private List<Guid> _cnvPaintItems;
        private double _cnvPaintAreaWidth;

        private Style _canvasTextBoxCurrentStyle;
        private Style _canvasGridCurrentStyle;
        private Style _canvasBorderCurrentStyle;
        private double _canvasItemsWidth;

        private Border _currCharContainer;
        private bool _isFirstPageOpening;

        #endregion //Fields

        #region Init Page

        public StartPage()
        {
            this.InitializeComponent();

            _cnvPaintItems = new List<Guid>();

            _btnLeftX = -1d;
            _btnRightX = -1d;

            _charContainerX = -1d;
            _charContainerY = -1d;

            _isFirstPageOpening = true;
            this.SizeChanged += OnPageSizeChanged;

            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Hiding += StartPageHiding;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (this.DataContext is StartPageViewModel)
            {
                _model = (StartPageViewModel)this.DataContext;
                ItemsViewModel["AllGroups"] = _model.AllGroups;
                this.DataContext = ItemsViewModel;
                
                if (_isFirstPageOpening)
                {
                    AddCharContainer(Utilities.CharContainerOrder.First, Constants.StarSymbol);
                    ComputeLeftRightButtonsPosition();
                    ComputeCharContainersPosFromLeftSide();
                    _isFirstPageOpening = false;
                }

                btnUndo.IsEnabled = _model.UndoEnabled;
                btnUndo.Command = _model.UndoCommand;
                btnRedo.IsEnabled = _model.RedoEnabled;
                btnRedo.Command = _model.RedoCommand;

                _model.LettesChanged += OnLettesChanged;
            }
        }

        #endregion //Init Page

        #region Paint Area Canvas

        #region Left Button

        private void BtnLeftManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var tempPosition = _btnLeftX + e.Delta.Translation.X;

            if (tempPosition > _horMiddlePoint - _canvasItemsWidth)
                tempPosition = _horMiddlePoint - _canvasItemsWidth;

            else if (tempPosition < 0)
                return;

            _btnLeftX = tempPosition;

            SetBtnLeftNewPosition();
        }

        private void BtnLeftManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            if (_btnLeftX == -1d)
                _btnLeftX = _horMiddlePoint - _canvasItemsWidth;
        }

        private void SetBtnLeftNewPosition()
        {
            var isAddedOrRemoved = false;

            if (IsCanAddCharContainer() == true)
            {
                AddCharContainer(Utilities.CharContainerOrder.First);
                isAddedOrRemoved = true;
            }
            else if (IsCanRemoveCharContainer() == true)
            {
                RemoveCharContainer((Border)cnvPaintArea.Children[StartPosCharContainersInCanvas]);
                isAddedOrRemoved = true;
            }

            if (isAddedOrRemoved == true)
            {
                ComputeLeftRightButtonsPosition();
                ComputeCharContainersPosFromLeftSide();
                isAddedOrRemoved = false;
            }

            btnLeft.ManipulationDelta -= BtnLeftManipulationDelta;

            Canvas.SetLeft(btnRight, _btnRightX);
            Canvas.SetLeft(btnLeft, _btnLeftX);

            btnLeft.ManipulationDelta += BtnLeftManipulationDelta;
        }

        private void BtnLeftManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            ComputeLeftRightButtonsPosition();

            btnLeft.ManipulationDelta -= BtnLeftManipulationDelta;

            Canvas.SetLeft(btnLeft, _btnLeftX);
            Canvas.SetLeft(btnRight, _btnRightX);

            btnLeft.ManipulationDelta += BtnLeftManipulationDelta;
        }

        #endregion //Left Button

        #region Right Button

        private void BtnRightManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var tempPosition = _btnRightX + e.Delta.Translation.X;

            if (tempPosition < _horMiddlePoint)
                tempPosition = _horMiddlePoint;
            else if (tempPosition > cnvPaintArea.ActualWidth)
                return;

            if (tempPosition > cnvPaintArea.ActualWidth - _canvasItemsWidth)
                tempPosition = cnvPaintArea.ActualWidth - _canvasItemsWidth;

            _btnRightX = tempPosition;

            SetBtnRightNewPosition();
        }

        private void BtnRightManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            if (_btnRightX == -1d)
                _btnRightX = _horMiddlePoint;
        }

        private void SetBtnRightNewPosition()
        {
            var isAddedOrRemoved = false;

            if (IsCanAddCharContainer() == true)
            {
                AddCharContainer(Utilities.CharContainerOrder.Last);
                isAddedOrRemoved = true;
            }
            else if (IsCanRemoveCharContainer() == true)
            {
                RemoveCharContainer((Border)cnvPaintArea.Children[cnvPaintArea.Children.Count - 1]);
                isAddedOrRemoved = true;
            }

            if (isAddedOrRemoved == true)
            {
                ComputeLeftRightButtonsPosition();
                ComputeCharContainersPosFromRightSide();
                isAddedOrRemoved = false;
            }

            Canvas.SetLeft(btnRight, _btnRightX);
            Canvas.SetLeft(btnLeft, _btnLeftX);
        }

        private void BtnRightManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            ComputeLeftRightButtonsPosition();

            Canvas.SetLeft(btnLeft, _btnLeftX);
            Canvas.SetLeft(btnRight, _btnRightX);
        }

        #endregion //Right Button

        #region CharContainer shift

        private void ComputeLeftRightButtonsPosition()
        {
            //value of shift to new position
            double delta = 0;

            if (double.IsNaN(_cnvPaintAreaWidth) || _cnvPaintAreaWidth <= 0)
                _cnvPaintAreaWidth = double.IsNaN(cnvPaintArea.Width) ? cnvPaintArea.ActualWidth : cnvPaintArea.Width;

            _horMiddlePoint = (double)(_cnvPaintAreaWidth / 2);
            //startPositionBtnLeft - max position to right of btnLeft
            double startPositionBtnLeft = _horMiddlePoint - _canvasItemsWidth;

            _btnLeftX = Canvas.GetLeft(btnLeft);
            _btnRightX = Canvas.GetLeft(btnRight);

            if (_btnRightX < _horMiddlePoint || _btnRightX > _cnvPaintAreaWidth)
                _btnRightX = _horMiddlePoint;

            if (_btnLeftX < 0 || _btnLeftX > _horMiddlePoint)
                _btnLeftX = startPositionBtnLeft;

            if (_model.Letters != null && _model.Letters.Count > 0)
            {
                var gap = _model.Letters.Count * _canvasItemsWidth;
                delta = gap / 2;
            }

            _btnLeftX = startPositionBtnLeft - delta;
            _btnRightX = _horMiddlePoint + delta;
        }

        private void ComputeCharContainersPosFromLeftSide()
        {
            if (_btnRightX - (_btnLeftX + _cnvPaintItems.Count * _canvasItemsWidth) < _canvasItemsWidth)
                return;

            var counter = 0;

            for (var j = cnvPaintArea.Children.Count - 1; j >= StartPosCharContainersInCanvas; j--)
            {
                var charContainer = cnvPaintArea.Children[j] as Border;

                if (charContainer != null)
                {
                    counter++;
                    Canvas.SetLeft(charContainer, _btnRightX - counter * _canvasItemsWidth);
                    ((TextBlock)((Grid)charContainer.Child).Children[1]).Text = (j - StartPosCharContainersInCanvas + 1).ToString();
                }
            }
        }

        private void ComputeCharContainersPosFromRightSide()
        {
            var charContainerPos = _btnLeftX;

            if (_btnRightX - (_btnLeftX + _cnvPaintItems.Count * _canvasItemsWidth) < _canvasItemsWidth)
                return;

            for (var j = StartPosCharContainersInCanvas; j < cnvPaintArea.Children.Count; j++)
            {
                var charContainer = cnvPaintArea.Children[j] as Border;

                if (charContainer != null)
                {
                    charContainerPos += _canvasItemsWidth;
                    Canvas.SetLeft(charContainer, charContainerPos);
                    ((TextBlock)((Grid)charContainer.Child).Children[1]).Text = (j - StartPosCharContainersInCanvas + 1).ToString();
                }
            }
        }

        #endregion //CharContainer shift

        #region Add CharContainer

        private bool IsCanAddCharContainer()
        {
            var coveredArea = (1 + _cnvPaintItems.Count) * _canvasItemsWidth;
            var allArea = _btnRightX - _btnLeftX;
            var uncoveredArea = allArea - coveredArea;

            if (uncoveredArea >= _canvasItemsWidth)
                return true;

            return false;
        }

        private void AddCharContainer(Utilities.CharContainerOrder charContainerOrder)
        {
            AddCharContainer(charContainerOrder, null);
        }

        private void AddCharContainer(Utilities.CharContainerOrder charContainerOrder, string letter)
        {
            var charContainer = CreateCharContainer(letter);

            if (_cnvPaintItems.Contains((Guid)charContainer.Tag) == true)
                return;

            if (charContainerOrder == Utilities.CharContainerOrder.First)
            {
                cnvPaintArea.Children.Insert(StartPosCharContainersInCanvas, charContainer);
                _cnvPaintItems.Insert(0, (Guid)charContainer.Tag);

                _model.Letters.Insert(0, ((TextBox)((Grid)charContainer.Child).Children[0]).Text);

                if (!_isFirstPageOpening)
                {
                    _model.SaveCurrentLettersState(_model.Letters, true);

                    btnUndo.IsEnabled = _model.UndoEnabled;
                    btnRedo.IsEnabled = _model.RedoEnabled;
                }
                else
                {
                    _model.SaveCurrentLettersState(new ObservableCollection<string> { "*" }, false);
                }
            }
            else if (charContainerOrder == Utilities.CharContainerOrder.Last)
            {
                cnvPaintArea.Children.Add(charContainer);
                _cnvPaintItems.Add((Guid)charContainer.Tag);

                _model.Letters.Add(((TextBox)((Grid)charContainer.Child).Children[0]).Text);

                if (!_isFirstPageOpening)
                {
                    _model.SaveCurrentLettersState(_model.Letters, true);

                    btnUndo.IsEnabled = _model.UndoEnabled;
                    btnRedo.IsEnabled = _model.RedoEnabled;
                }
                else
                {
                    _model.SaveCurrentLettersState(new ObservableCollection<string> { "*" }, false);
                }
            }
        }

        #endregion //Add CharContainer

        #region Remove CharContainer

        private bool IsCanRemoveCharContainer()
        {
            var coveredArea = _canvasItemsWidth + _cnvPaintItems.Count * _canvasItemsWidth;
            var allArea = _btnRightX - _btnLeftX;
            var overCoveredArea = coveredArea - allArea;

            if (overCoveredArea > _canvasItemsWidth / 10)
                return true;

            return false;
        }

        private void RemoveCharContainer(Border charContainer)
        {
            var elementIndex = cnvPaintArea.Children.IndexOf(charContainer);

            if (elementIndex == -1)
                return;

            cnvPaintArea.Children.RemoveAt(elementIndex);
            elementIndex = _cnvPaintItems.FindIndex(t => t == (Guid)charContainer.Tag);
            
            if (elementIndex == -1)
                return;

            _cnvPaintItems.RemoveAt(elementIndex);
            _model.Letters.RemoveAt(elementIndex);

            _model.SaveCurrentLettersState(_model.Letters, true);

            btnUndo.IsEnabled = _model.UndoEnabled;
            btnRedo.IsEnabled = _model.RedoEnabled;
        }

        #endregion //Remove CharContainer

        #region CharContainer Manipulations

        private void CharContainerManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            HideKeywordsPopup();

            var charContainer = sender as Border;
            var posX = _charContainerX;
            var posY = _charContainerY;

            if (Math.Abs(posY - _defCharContainerY) > charContainer.ActualHeight)
            {
                RemoveCharContainer(charContainer);
                ComputeLeftRightButtonsPosition();
                ComputeCharContainersPosFromRightSide();
                Canvas.SetLeft(btnLeft, _btnLeftX);
                Canvas.SetLeft(btnRight, _btnRightX);
            }
            else
            {
                charContainer.Opacity = 1;
                charContainer.BorderThickness = new Thickness(2);

                _charContainerX = _defCharContainerX;
                _charContainerY = _defCharContainerY;

                Canvas.SetTop(charContainer, _charContainerY);
                Canvas.SetLeft(charContainer, _charContainerX);
            }
        }

        private void CharContainerManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            HideKeywordsPopup();

            _charContainerX += e.Delta.Translation.X;
            _charContainerY += e.Delta.Translation.Y;

            Canvas.SetLeft((UIElement)sender, _charContainerX);
            Canvas.SetTop((UIElement)sender, _charContainerY);
        }

        private void CharContainerManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            var charContainer = sender as Border;

            _charContainerX = Canvas.GetLeft(charContainer);
            _charContainerY = Canvas.GetTop(charContainer);

            _defCharContainerX = _charContainerX;
            _defCharContainerY = _charContainerY;

            HideKeywordsPopup();
        }

        #endregion //CharContainer Manipulations

        #region Create CharContainers or Change Context

        private Border CreateCharContainer(string letter)
        {
            var txtBox = new TextBox();
            txtBox.Style = _canvasTextBoxCurrentStyle;
            txtBox.Text = string.IsNullOrEmpty(letter) ? Constants.QuestionSymbol : letter;
            txtBox.TextChanged += CharContainerTextChanged;

            var txtBlock = new TextBlock();
            txtBlock.FontSize = 15;
            txtBlock.Foreground = new SolidColorBrush(Colors.Gray);
            txtBlock.HorizontalAlignment = HorizontalAlignment.Right;
            txtBlock.Margin = new Thickness(0, 3, 3, 0);
            txtBlock.Text = (_cnvPaintItems.Count + 1).ToString();
            txtBlock.IsTextSelectionEnabled = false;
            txtBlock.IsTapEnabled = false;

            var grid = new Grid();
            grid.Style = _canvasGridCurrentStyle;
            grid.Children.Add(txtBox);
            grid.Children.Add(txtBlock);

            var border = new Border();
            border.Tag = Guid.NewGuid();
            txtBox.Tag = border.Tag;
            border.Name = txtBox.Tag.ToString();
            border.Child = grid;
            border.Style = _canvasBorderCurrentStyle;
            border.GotFocus += CharContainerGotFocus;

            Canvas.SetTop(border, Canvas.GetTop(btnLeft));
            Canvas.SetLeft(border, _horMiddlePoint);

            border.ManipulationMode = ManipulationModes.All;
            border.ManipulationStarting += CharContainerManipulationStarting;
            border.ManipulationDelta += CharContainerManipulationDelta;
            border.ManipulationCompleted += CharContainerManipulationCompleted;

            return border;
        }

        private void CharContainerGotFocus(object sender, RoutedEventArgs e)
        {
            _currCharContainer = (Border)sender;

            ShowKeywordsPopup();
        }

        private void CharContainerTextChanged(object sender, TextChangedEventArgs e)
        {
            if (_model == null)
                return;

            var txtBox = sender as TextBox;
            var text = txtBox.Text;
            var newSymbol = string.Empty;

            var chArr = (_model.Letters[_cnvPaintItems.IndexOf((Guid)txtBox.Tag)]).ToCharArray();

            var oldSymbol = chArr.Length == 0 ? '?' : chArr[0];

            chArr = text.ToCharArray();
            for (var i = 0; i < chArr.Length; i++)
            {
                if (chArr[i] == oldSymbol)
                    continue;

                newSymbol = chArr[i].ToString();
                break;
            }

            txtBox.TextChanged -= CharContainerTextChanged;

            txtBox.Text = newSymbol;

            _model.Letters[_cnvPaintItems.IndexOf((Guid)txtBox.Tag)] = newSymbol;

            txtBox.TextChanged += CharContainerTextChanged;

            HideKeywordsPopup();

            _model.SaveCurrentLettersState(_model.Letters, true);

            btnUndo.IsEnabled = _model.UndoEnabled;
            btnRedo.IsEnabled = _model.RedoEnabled;
        }

        private void ShowKeywordsPopup()
        {
            if (keywordsPopup == null || _currCharContainer == null)
                return;

            Canvas.SetLeft(keywordsPopup, Canvas.GetLeft(_currCharContainer) - _canvasItemsWidth);
            Canvas.SetTop(keywordsPopup, Canvas.GetTop(_currCharContainer) + _canvasItemsWidth * (1.2));

            keywordsPopup.Visibility = Visibility.Visible;
            keywordsPopup.IsOpen = true;
        }

        private void HideKeywordsPopup()
        {
            if (keywordsPopup == null)
                return;

            keywordsPopup.IsOpen = false;
            keywordsPopup.Visibility = Visibility.Collapsed;
        }

        private void BtnQuestionSymbolTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_currCharContainer == null)
                return;

            ((TextBox)((Grid)_currCharContainer.Child).Children[0]).Text = "?";
            HideKeywordsPopup();

        }

        private void BtnStarSymbolTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_currCharContainer == null)
                return;

            ((TextBox)((Grid)_currCharContainer.Child).Children[0]).Text = "*";
            HideKeywordsPopup();
        }

        private void BtnPlusSymbolTapped(object sender, TappedRoutedEventArgs e)
        {
            if (_currCharContainer == null)
                return;

            ((TextBox)((Grid)_currCharContainer.Child).Children[0]).Text = "+";
            HideKeywordsPopup();
        }

        private void StartPageHiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            HideKeywordsPopup();
        }

        #endregion //Create CharContainers or Change Context

        #endregion //Paint Area Canvas

        #region Search

        private async void BtnSearchTapped(object sender, TappedRoutedEventArgs e)
        {
            _model.ShowProgressBar(Windows.UI.Xaml.Visibility.Visible);

            await _model.FindMatchedWords();

            btnUndo.IsEnabled = _model.UndoEnabled;
            btnRedo.IsEnabled = _model.RedoEnabled;

            if (this.DataContext is StartPageViewModel)
            {
                _model = (StartPageViewModel)this.DataContext;
                ItemsViewModel["AllGroups"] = _model.AllGroups;
                groupedItemsSource.Source = ItemsViewModel["AllGroups"];
                this.DataContext = _model;
            }
            else if ((this.DataContext is IObservableMap<string, object>) == true)
            {
                ItemsViewModel["AllGroups"] = _model.AllGroups;
                groupedItemsSource.Source = ItemsViewModel["AllGroups"];
                this.DataContext = _model;
            }

            _model.ShowProgressBar(Windows.UI.Xaml.Visibility.Collapsed);
        }

        #endregion //Search

        #region UndoRedo Changes

        private void OnLettesChanged(object sender, EventArgs e)
        {
            if (_model == null)
                return;

            UpdateLettersLayout();

            btnUndo.IsEnabled = _model.UndoEnabled;
            btnRedo.IsEnabled = _model.RedoEnabled;

            this.DataContext = _model;
        }

        private void UpdateLettersLayout()
        {
            _cnvPaintItems.Clear();

            for (var i = StartPosCharContainersInCanvas; i < cnvPaintArea.Children.Count; i++)
            {
                if (cnvPaintArea.Children[i] is Border)
                {
                    cnvPaintArea.Children.RemoveAt(i);
                    i = 0;
                }
            }

            for (var i = 0; i < _model.Letters.Count; i++)
            {
                var charContainer = CreateCharContainer(_model.Letters[i]);
                if (_cnvPaintItems.Contains((Guid)charContainer.Tag) == true)
                    return;

                cnvPaintArea.Children.Add(charContainer);
                _cnvPaintItems.Add((Guid)charContainer.Tag);
            }

            SetLeftOfCanvasItems();
        }

        #endregion //UndoRedo Changes

        #region Results panel

        private void HyperlinkHeaderGroupClick(object sender, RoutedEventArgs e)
        {
            var btn = sender as HyperlinkButton;

            if (btn == null || (btn.Content is string) == false)
                return;

            var content = (string)btn.Content;

            _model.ShowWordsWithTheSameLengthView((string)btn.Content);
        }

        #endregion //Results panel

        #region Change ViewSate

        private void OnPageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetStylesForSelectedAppView();

            SetTopOfCanvasItems();
            SetLeftOfCanvasItems();
        }

        private void SetTopOfCanvasItems()
        {
            var topPoint = cnvPaintArea.Height / 2 - _canvasItemsWidth / 2;
            Canvas.SetTop(btnLeft, topPoint);
            Canvas.SetTop(btnRight, topPoint);

            for (var i = StartPosCharContainersInCanvas; i < cnvPaintArea.Children.Count; i++)
            {
                if (cnvPaintArea.Children[i] is Border)
                    Canvas.SetTop(cnvPaintArea.Children[i], topPoint);
            }

            var top = cnvPaintArea.Height - (_canvasItemsWidth + topPoint);
        }

        private void SetLeftOfCanvasItems()
        {
            ComputeLeftRightButtonsPosition();

            Canvas.SetLeft(btnLeft, _btnLeftX);
            Canvas.SetLeft(btnRight, _btnRightX);

            var currPos = _btnLeftX;

            for (var i = StartPosCharContainersInCanvas; i < cnvPaintArea.Children.Count; i++)
            {
                var charContainer = cnvPaintArea.Children[i] as Border;

                if (charContainer == null)
                    continue;

                currPos += _canvasItemsWidth;

                Canvas.SetLeft(charContainer, currPos);
            }
        }

        private void SetStylesForSelectedAppView()
        {
            if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Filled)
            {
                searchGrid.Visibility = Visibility.Visible;
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFilledView"];
                cnvPaintArea.Style = (Style)App.Current.Resources["CanvasFilledStyle"];
                btnSearch.Style = (Style)App.Current.Resources["SearchButtonStyleFilledView"];
                btnLeft.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnRight.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnQuestionSymbol.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnStarSymbol.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnPlusSymbol.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                _canvasTextBoxCurrentStyle = (Style)App.Current.Resources["CanvasTextBoxStyleFilledView"];
                _canvasGridCurrentStyle = (Style)App.Current.Resources["CanvasGridStyleFilledView"];
                _canvasBorderCurrentStyle = (Style)App.Current.Resources["CanvasBorderStyleFilledView"];
                _canvasItemsWidth = (double)App.Current.Resources["CanvasItemsWidthFilledView"];
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.FullScreenLandscape)
            {
                searchGrid.Visibility = Visibility.Visible;
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFilledView"];
                cnvPaintArea.Style = (Style)App.Current.Resources["CanvasFilledStyle"];
                btnSearch.Style = (Style)App.Current.Resources["SearchButtonStyleFilledView"];
                btnLeft.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnRight.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnQuestionSymbol.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnStarSymbol.Style = (Style)App.Current.Resources["CanvasButtonFilledStyle"];
                btnPlusSymbol.Style = (Style)App.Current.Resources["CanvasButtonFullScreenPortraitStyle"];
                _canvasTextBoxCurrentStyle = (Style)App.Current.Resources["CanvasTextBoxStyleFilledView"];
                _canvasGridCurrentStyle = (Style)App.Current.Resources["CanvasGridStyleFilledView"];
                _canvasBorderCurrentStyle = (Style)App.Current.Resources["CanvasBorderStyleFilledView"];
                _canvasItemsWidth = (double)App.Current.Resources["CanvasItemsWidthFilledView"];
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.Snapped)
            {
                searchGrid.Visibility = Visibility.Collapsed;
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleSnappedView"];
                _canvasItemsWidth = (double)App.Current.Resources["CanvasItemsWidthSnappedView"];
                if (WordsSnappedView != null && _model != null)
                    WordsSnappedView.ItemsSource = _model.AllGroups;
            }
            else if (Windows.UI.ViewManagement.ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
            {
                searchGrid.Visibility = Visibility.Visible;
                MatchedWordsHeader.Style = (Style)App.Current.Resources["HeaderTextBlockStyleFullScreenPortraitView"];
                cnvPaintArea.Style = (Style)App.Current.Resources["CanvasFullScreenPortraitStyle"];
                btnSearch.Style = (Style)App.Current.Resources["SearchButtonStyleFullScreenPortraitView"];
                btnLeft.Style = (Style)App.Current.Resources["CanvasButtonFullScreenPortraitStyle"];
                btnRight.Style = (Style)App.Current.Resources["CanvasButtonFullScreenPortraitStyle"];
                btnQuestionSymbol.Style = (Style)App.Current.Resources["CanvasButtonFullScreenPortraitStyle"];
                btnStarSymbol.Style = (Style)App.Current.Resources["CanvasButtonFullScreenPortraitStyle"];
                btnPlusSymbol.Style = (Style)App.Current.Resources["CanvasButtonFullScreenPortraitStyle"];
                _canvasTextBoxCurrentStyle = (Style)App.Current.Resources["CanvasTextBoxStyleFullScreenPortraitView"];
                _canvasGridCurrentStyle = (Style)App.Current.Resources["CanvasGridStyleFullScreenPortraitView"];
                _canvasBorderCurrentStyle = (Style)App.Current.Resources["CanvasBorderStyleFullScreenPortraitView"];
                _canvasItemsWidth = (double)App.Current.Resources["CanvasItemsWidthFullScreenPortraitView"];
            }

            _cnvPaintAreaWidth = double.IsNaN(cnvPaintArea.Width) ? this.ActualWidth - 10 : cnvPaintArea.Width;

            for (var i = StartPosCharContainersInCanvas; i < cnvPaintArea.Children.Count; i++)
            {
                var charContainer = cnvPaintArea.Children[i] as Border;

                if (charContainer == null)
                    continue;

                charContainer.Style = _canvasBorderCurrentStyle;
                ((TextBox)((Grid)charContainer.Child).Children[0]).Style = _canvasTextBoxCurrentStyle;
                ((TextBox)((Grid)charContainer.Child).Children[0]).Height = charContainer.Height - 2 * charContainer.Margin.Left;
                ((TextBox)((Grid)charContainer.Child).Children[0]).Width = charContainer.Width - 2 * charContainer.Margin.Left;
            }
        }

        #endregion //Change ViewSate
    }
}