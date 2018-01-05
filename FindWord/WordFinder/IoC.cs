using MetroIoc;
using FindWord.Messaging;
using FindWord.Services;
using FindWord.ViewModels;

namespace FindWord
{
    class IoC
    {
        private static MetroContainer container = new MetroContainer();
        public static MetroContainer Container{get { return container; }}

        public static IContainer BuildContainer()
        {
            container.RegisterInstance(container);
            container.RegisterInstance<IContainer>(container);

            container.Register<IHub, MessageHub>();
            container.Register<IDialogService, DialogService>();
            container.Register<INavigationService, NavigationService>(null, registration: new Singleton());

            container.Register<UndoRedoService>(null, registration: new Singleton());
            container.Register<SearchService>(null, registration: new Singleton());

            container.Register<ShellViewModel>(null, registration: new Singleton());
            container.Register<StartPageViewModel>(null, registration: new Singleton());
            container.Register<WordsWithTheSameLengthViewModel>(null, registration: new Singleton());

            RegisterHandlers(container);

            return container;
        }

        private static void RegisterHandlers(IContainer container)
        {
            container.Register<IHandler<ShowStartPageMessage>, ShowStartPageHandler>();
            container.Register<IHandler<ShowWordsWithTheSameLengthPageMessage>, ShowWordsWithTheSameLengthPageHandler>();

            container.Register<IHandler<ShowProgressBarMessage>, ShowProgressBarHandler>();
            container.Register<IHandler<ShowProgressRingMessage>, ShowProgressRingHandler>();
        }
    }
}