using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.UI.Popups;
using FindWord.Messaging;

namespace FindWord.Services
{
    public class DialogService : IDialogService
    {
        private readonly ResourceLoader _resourceLoader = new ResourceLoader();

        private static bool _isShowing;
        private IHub _messageHub;

        public DialogService(IHub messageHub)
        {
            _messageHub = messageHub;
        }

        #region IDialogService interface implementation

        public async Task ShowMessageAsync(string message)
        {
            if (_isShowing == false)
                _isShowing = true;
            else
                return;

            var dialog = new MessageDialog(message);
            var task = dialog.ShowAsync();           
            await task;

            if (task.Status == AsyncStatus.Completed)
                _isShowing = false;
        }

        public async Task ShowMessageWithOkCommandAsync(string message)
        {
            if (_isShowing == false)
                _isShowing = true;
            else
                return;

            var dialog = new MessageDialog(message);

            dialog.Commands.Add(new UICommand("Ok",new UICommandInvokedHandler(OkButtonClick)));
            dialog.Commands.Add(new UICommand("Close", new UICommandInvokedHandler(CloseButtonClick)));

            // Set the command that will be invoked by default
            dialog.DefaultCommandIndex = 0;

            // Set the command to be invoked when escape is pressed
            dialog.CancelCommandIndex = 1;
            
            var task = dialog.ShowAsync();
            await task;

            if (task.Status == AsyncStatus.Completed)
                _isShowing = false;
        }

        public async Task ShowResourceMessageAsync(string key)
        {
            if (_isShowing == false)
                _isShowing = true;
            else
                return;

            var dialog = new MessageDialog(_resourceLoader.GetString(key));
            var task = dialog.ShowAsync();
            await task;

            if (task.Status == AsyncStatus.Completed)
                _isShowing = false;
        }

        #endregion //IDialogService interface implementation

        private void OkButtonClick(IUICommand command)
        {
            
        }
        private void CloseButtonClick(IUICommand command)
        {
            
        }
    }
}