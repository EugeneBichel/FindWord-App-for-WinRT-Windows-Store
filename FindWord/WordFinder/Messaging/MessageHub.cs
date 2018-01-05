using System.Threading.Tasks;
using MetroIoc;

namespace FindWord.Messaging
{
    public class MessageHub : IHub
    {
        private readonly IContainer _container;

        public MessageHub(IContainer container)
        {
            _container = container;
        }

        public async Task Send<TMessage>(TMessage message) where TMessage : IMessage
        {
            var handler = _container.TryResolve<IHandler<TMessage>>(null);
            if (handler != null)
            {
                handler.Handle(message);
                return;
            }

            var asyncHandler = _container.TryResolve<IAsyncHandler<TMessage>>(null);
            if (asyncHandler != null)
            {
                await asyncHandler.HandleAsync(message);
                return;
            }
        }

        public async Task Send<TMessage>(TMessage message,string key) where TMessage : IMessage
        {
            var handler = _container.TryResolve<IHandler<TMessage>>(key);
            if (handler != null)
            {
                handler.Handle(message);
                return;
            }

            var asyncHandler = _container.TryResolve<IAsyncHandler<TMessage>>(key);
            if (asyncHandler != null)
            {
                await asyncHandler.HandleAsync(message);
                return;
            }
        }
    }
}