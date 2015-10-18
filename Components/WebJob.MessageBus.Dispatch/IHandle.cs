namespace WebJob.MessageBus.Dispatch
{
    public interface IHandle<in T>
    {
        void Handle(T message);
    }
}