using Microsoft.Azure.WebJobs.Host;
using Microsoft.Practices.Unity;

namespace WebJob.MessageBus
{
    public class UnityJobActivator : IJobActivator
    {
        private readonly IUnityContainer _container;

        public UnityJobActivator(IUnityContainer container)
        {
            _container = container;
        }

        public T CreateInstance<T>()
        {
            return _container.Resolve<T>();
        }
    }
}