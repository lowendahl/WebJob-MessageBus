using System;
using WebJob.MessageBus.Example.Messages;

namespace WebJob.MessageBus.Example.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MessageBusClient();

            Console.WriteLine("Press any key to send a message or ESC to exit.");

            while(Console.ReadKey().Key != ConsoleKey.Escape)
            {
                Console.WriteLine("Sending message.");
                client.SendMessage<NewImageAdded>(new NewImageAdded()
                {
                    ImageName = @"01bb8c3d-9320-452f-b810-04e7e72c4a9e/0860.JPG"
                });
            }
        }
    }
}
