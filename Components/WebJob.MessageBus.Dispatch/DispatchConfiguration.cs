using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace WebJob.MessageBus.Dispatch
{
    public class DispatchConfiguration
    {
        private readonly IUnityContainer _container;
        private readonly Dictionary<Type, List<Type>> _messageHandlers;

        public DispatchConfiguration(IUnityContainer container)
        {
            _container = container;
            _messageHandlers = new Dictionary<Type, List<Type>>();
        }

        public void ConfigureHandlersFrom(Assembly handlerAssembly)
        {
            Trace.WriteLine("[Dispatch] Registering listeners:");

            var handlers = handlerAssembly.GetTypes()
                .Where(type => type.GetInterfaces()
                    .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IHandle<>)) != null)
                .ToList();

            foreach (var handlerType in handlers)
            {
                handlerType.GetInterfaces()
                      .Where(i => i.GetGenericTypeDefinition() == typeof(IHandle<>))
                      .Select(i => i.GetGenericArguments().First())
                      .ForEach(messageType => Add(messageType, handlerType));
            }

            Trace.WriteLine("[Dispatch] Listener registration done.");
        }

        public void Add(Type messageType, Type handlerType)
        {
            if (!_messageHandlers.ContainsKey(messageType))
                _messageHandlers.Add(messageType, new List<Type>());

            _messageHandlers[messageType].Add((handlerType));
            _container.RegisterType(handlerType);

            Trace.WriteLine($"[Dispatch] Listener added: {handlerType.Name} is now listening to {messageType.Name}");
        }

        public IEnumerable<Type> GetHandlersFor<TMessage>()
        {
            return _messageHandlers[typeof(TMessage)];
        }
    }
}