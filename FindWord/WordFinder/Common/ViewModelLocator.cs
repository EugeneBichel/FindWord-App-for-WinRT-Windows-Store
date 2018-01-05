using System;
using MetroIoc;
using FindWord.Messaging;
using FindWord.Services;
using FindWord.ViewModels;

namespace FindWord.Common
{
    public class ViewModelLocator
    {
        private Lazy<IContainer> container;

        public ViewModelLocator()
        {
            container = new Lazy<IContainer>(IoC.BuildContainer);
        }

        public IContainer Container
        {
            get { return container.Value; }
        }

        public INavigationService NavigationService
        {
            get { return Container.Resolve<INavigationService>(); }
        }

        public IHub Hub
        {
            get { return Container.Resolve<IHub>(); }
        }

        public IDialogService DialogService
        {
            get { return Container.Resolve<IDialogService>(); }
        }

        public ShellViewModel ShellViewModel
        {
            get { return Container.Resolve<ShellViewModel>(); }
        }

        public StartPageViewModel StartPageViewModel
        {
            get { return Container.Resolve<StartPageViewModel>(); }
        }

        public WordsWithTheSameLengthViewModel WordsWithTheSameLengthViewModel
        {
            get { return Container.Resolve<WordsWithTheSameLengthViewModel>(); }
        }

        public UndoRedoService UndoRedoService
        {
            get { return Container.Resolve<UndoRedoService>(); }
        }

        public SearchService SearchService
        {
            get { return Container.Resolve<SearchService>(); }
        }

        public AppState AppState
        {
            get { return Container.Resolve<AppState>(); }
        }
    }
}