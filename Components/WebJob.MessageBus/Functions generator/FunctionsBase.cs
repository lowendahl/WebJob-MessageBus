using WebJob.MessageBus.Dispatch;

namespace WebJob.MessageBus
{
    public abstract class FunctionsBase
    {
        protected readonly MessageDispatcher Dispatcher;

        protected FunctionsBase(MessageDispatcher dispatcher)
        {
           Dispatcher = dispatcher;
        }
    }
}