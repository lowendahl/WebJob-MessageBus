using WebJob.MessageBus.Dispatch;

namespace WebJob.MessageBus.Example.Messages
{
    public class NewImageAdded : Message
    {
        public string ImageName { get; set; }
    }
}