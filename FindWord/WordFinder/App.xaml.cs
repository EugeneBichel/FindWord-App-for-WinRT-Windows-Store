using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using FindWord.Common;
using FindWord.Messaging;
using FindWord.Services;
using FindWord.Views;

namespace FindWord
{
    sealed partial class App : Application
    {
        #region Fields

        private Shell _shell;

        public static ViewModelLocator ViewModelLocator { get; private set; }
        public static CoreDispatcher Dispatcher { get; set; }

        #endregion //Fields

        #region Constructor

        public App()
        {
            InitializeComponent();
            this.UnhandledException += AppUnhandledException;
            this.Suspending += AppSuspending;
        }

        #endregion //Constructor

        #region App lifecycle

        /// <summary>
        /// Terminated by the system (for example, because of resource constraints) - Restore session data 
        /// Closed by the user ClosedByUser - Start with default data  
        /// Unexpectedly terminated, or app has not run since the user’s session started - Start with default data  
        /// </summary>
        protected async override void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                var appState = await AppState.LoadStateAsync();
                IoC.Container.RegisterInstance<AppState>(appState);
                ViewModelLocator = (ViewModelLocator)App.Current.Resources["ViewModelLocator"];

                await EnsureShellAsync(args);
            }
            catch (Exception ex)
            {
                ///http://social.msdn.microsoft.com/Forums/en/winappswithcsharp/thread/bea154b0-08b0-4fdc-be31-058d9f5d1c4e
                ((App)App.Current).OnUnhandledException(ex);
            }
        }

        private async void AppSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                await ViewModelLocator.AppState.SaveStateAsync();

                deferral.Complete();
            }
            catch (Exception ex)
            {
                ///http://social.msdn.microsoft.com/Forums/en/winappswithcsharp/thread/bea154b0-08b0-4fdc-be31-058d9f5d1c4e
                ((App)App.Current).OnUnhandledException(ex);
            }
        }

        private async Task EnsureShellAsync(LaunchActivatedEventArgs args)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                //ViewModelLocator.NavigationService.Refresh();
                Window.Current.Activate();
                return;
            }

            _shell = new Shell();
            App.ViewModelLocator.NavigationService.InitializeFrame(_shell.Frame);

            SplashScreen splashScreen = args.SplashScreen;
            ExtendedSplash eSplash = new ExtendedSplash(splashScreen, false, args, _shell);
            splashScreen.Dismissed += new TypedEventHandler<SplashScreen, object>(eSplash.DismissedEventHandler);
            Window.Current.Content = eSplash;
            Window.Current.Activate();

            if (args.TileId == "App")
            {
                //Terminated - Restore session data 
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                    await ViewModelLocator.NavigationService.LoadSavedSession();
                else // ClosedByUser - Start with default data; NotRunning - Start with default data 
                    await ViewModelLocator.Hub.Send<ShowStartPageMessage>(new ShowStartPageMessage());
            }
        }

        private async Task EnsureShellAsync(ApplicationExecutionState previousState)
        {
            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (previousState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                ViewModelLocator.NavigationService.Navigate(typeof(Shell));
                return;
            }

            _shell = new Shell();
            ViewModelLocator.NavigationService.InitializeFrame(_shell.Frame);
            Window.Current.Content = _shell;
            Window.Current.Activate();
            ViewModelLocator.NavigationService.Navigate(typeof(Shell));

            //Terminated - Restore session data 
            if (previousState == ApplicationExecutionState.Terminated)
                await ViewModelLocator.NavigationService.LoadSavedSession();
            else // ClosedByUser - Start with default data; NotRunning - Start with default data  
            {
                await ViewModelLocator.Hub.Send<ShowStartPageMessage>(new ShowStartPageMessage());
            }
            App.Dispatcher = Window.Current.CoreWindow.Dispatcher;
        }

        #endregion //App lifecycle

        #region Exception handling

        public async void OnUnhandledException(Exception ex)
        {
            Debug.WriteLine(ex);
            Debug.WriteLine(ex.StackTrace);

            if (App.Dispatcher == null)
                return;

            var dispatcher = App.Dispatcher;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, (DispatchedHandler)(async () =>
            {
#if DEBUG
                var baseEx = ex.GetBaseException();
                var message = ex.Message;
                if (baseEx != ex)
                {
                    message += "\r\n\r\n" + baseEx.Message;
                }
                await ViewModelLocator.DialogService.ShowMessageAsync(message);
#else
                await ViewModelLocator.DialogService.ShowResourceMessageAsync("Exception_UnhandledException");
                Exit();
#endif
            }));
        }

        private void AppUnhandledException(object sender, UnhandledExceptionEventArgs ex)
        {
            ex.Handled = true;
            OnUnhandledException(ex.Exception);
        }

        #endregion //Exception handling
    }
}