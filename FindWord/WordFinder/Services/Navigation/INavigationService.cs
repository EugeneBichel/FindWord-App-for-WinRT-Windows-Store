using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FindWord.Services
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;
        event Action<bool> CanGoBackNavigating;

        void InitializeFrame(Frame frame);
        void Navigate(Type source, object parameter = null);
        void Refresh();
        Task LoadSavedSession();
        void ShowProgressBar(Visibility progressBarVisibility);
        void ShowProgressRing(Visibility progressRingVisibility);

        ICommand GoBackCommand { get; }
        ViewType CurrentView { get; }
        Stack<ViewType> NavigationStack { get; }
    }
}