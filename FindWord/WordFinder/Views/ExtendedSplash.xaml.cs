using System;
using Windows.ApplicationModel.Activation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FindWord.Data;

namespace FindWord.Views
{
    partial class ExtendedSplash
    {
        private bool dismissed = false; // Variable to track splash screen dismissal status.
        private SplashScreen splash; // Variable to hold the splash screen object.

        public ExtendedSplash(SplashScreen splashScreen, bool dismissed, LaunchActivatedEventArgs args, Shell shell)
        {
            this.InitializeComponent();
            //Save system copy of splash screen for future reference and activation args
            splash = splashScreen;
            this.dismissed = dismissed;

            //move/resize local image exactly as on system splash screen
            extendedSplashImage.SetValue(Canvas.LeftProperty, splash.ImageLocation.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splash.ImageLocation.Y);
            extendedSplashImage.Height = splash.ImageLocation.Height;
            extendedSplashImage.Width = splash.ImageLocation.Width;

            //subscribe to resize event to handle new size of splash screen while active
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplashOnResize);
            this.Loaded += ExtendedSplashLoaded;

            //Simulate work by running timer...
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, arg) =>
            {
                if (this.dismissed)
                {
                    timer.Stop();
                    timer = null;// When finished, navigate to real first page

                    Window.Current.Content = shell;
                    App.Dispatcher = Window.Current.CoreWindow.Dispatcher;
                    App.ViewModelLocator.NavigationService.Navigate(typeof(Shell));
                    Window.Current.Activate();
                }
            };
            timer.Start();
        }

        private async void ExtendedSplashLoaded(object sender, RoutedEventArgs e)
        {
            var wordCategories = await Repository.GetAll();
            Repository.WordsWithLength = wordCategories.GetWordsWithLength();
        }

        private void ExtendedSplashOnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the splash screen image coordinates
            if (splash != null)
            {
                // Re-position the extended splash screen image due to window resize event.
                this.extendedSplashImage.SetValue(Canvas.LeftProperty, splash.ImageLocation.X);
                this.extendedSplashImage.SetValue(Canvas.TopProperty, splash.ImageLocation.Y);
                this.extendedSplashImage.Height = splash.ImageLocation.Height;
                this.extendedSplashImage.Width = splash.ImageLocation.Width;
            }
        }

        internal void DismissedEventHandler(SplashScreen sender, object e)
        {
            //Utilities.ThreadSleep(TimeSpan.FromSeconds(7));
            Utilities.ThreadSleep(TimeSpan.FromSeconds(4));
            this.dismissed = true;
        }
    }
}