using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.Unity;

namespace WebJob.MessageBus.Dispatch
{
    public class MessageDispatcher
    {
        private readonly IUnityContainer _container;
        private readonly DispatchConfiguration _configuration;

        public MessageDispatcher(IUnityContainer container, DispatchConfiguration configuration)
        {
            _container = container;
            _configuration = configuration;
        }

        public void Dispatch<TMessage>(TMessage message) where TMessage:Message
        {
            _configuration.GetHandlersFor<TMessage>()
                          .AsParallel()
                          .ForAll(handler => Dispatch(handler, message));
        }

        private void Dispatch<T>(Type handler, T message) where T: Message
        {
            Trace.WriteLine($"[Dispatch]  Dispatching message {message.GetType().Name} to {handler.Name}");
            var handlerObj = (IHandle<T>) _container.Resolve(handler);
            handlerObj.Handle(message);
        }
    }
}