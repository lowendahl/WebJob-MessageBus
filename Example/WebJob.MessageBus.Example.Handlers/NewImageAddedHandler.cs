using System;
using WebJob.MessageBus.Dispatch;
using WebJob.MessageBus.Example.Messages;

namespace WebJob.MessageBus.Example.Handlers
{
    public class NewImageAddedHandler : IHandle<NewImageAdded>
    {
        public void Handle(NewImageAdded message)
        {
            Console.WriteLine($"Handling image: {message.ImageName}");
        }
    }
}