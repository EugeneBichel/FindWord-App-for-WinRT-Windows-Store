using System.Threading.Tasks;

namespace FindWord.Services
{
    public interface IDialogService
    {
        Task ShowMessageAsync(string message);
        Task ShowMessageWithOkCommandAsync(string message);
        Task ShowResourceMessageAsync(string key);
    }
}