using System.Threading.Tasks;

namespace FindWord.Messaging
{
    public interface IHub
    {
        Task Send<TMessage>(TMessage message,string key=null) where TMessage : IMessage;
    }
}