using Microsoft.Azure.WebJobs;
using Microsoft.Practices.Unity;
using WebJob.MessageBus.Dispatch;

using WebJob.MessageBus.Example.Handlers;
using WebJob.MessageBus.Example.Messages;

namespace WebJob.MessageBus
{
    class Program
    {
        private static JobHostConfiguration _configuration;
        static void Main()
        {
            Initialize();
            Run();
        }

        private static void Run()
        {
            var host = new JobHost(_configuration);
            host.RunAndBlock();
        }

        private static void Initialize()
        {
            RegisterDispatchConfiguration();

            var messageAssembly = typeof (NewImageAdded).Assembly;
            _configuration = new JobHostConfiguration
            {
                TypeLocator = new DynamicTypeLocator(messageAssembly),
                JobActivator = new UnityJobActivator(UnityConfiguration.GetConfiguredContainer())
            };
        }

        private static void RegisterDispatchConfiguration()
        {
            var container = UnityConfiguration.GetConfiguredContainer();
            var dispatchConfiguration = container.Resolve<DispatchConfiguration>();
            var handlerAssembly = typeof (NewImageAddedHandler).Assembly;

            dispatchConfiguration.ConfigureHandlersFrom(handlerAssembly);
            container.RegisterInstance(dispatchConfiguration);
        }
    }
}
