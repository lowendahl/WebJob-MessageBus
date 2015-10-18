using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using WebJob.MessageBus.Dispatch;

namespace WebJob.MessageBus.Example.Client
{
    public class MessageBusClient
    {
        private readonly CloudQueueClient _client;

        public MessageBusClient()
        {
            var blobStorageConnectionString = ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(blobStorageConnectionString);
            _client = storageAccount.CreateCloudQueueClient();
        }

        public void SendMessage<TMessage>(TMessage messageInstance) where TMessage: Message
        {
            var queue = GetQueueFor<TMessage>();
            var messageContents = GetMessageContents(messageInstance);
            var messageToQueue = new CloudQueueMessage(messageContents);
            queue.AddMessage(messageToQueue);
        }

        private static string GetMessageContents(Message message)
        {
            return JsonConvert.SerializeObject(message);
        }

        private CloudQueue GetQueueFor<TMessage>()
        {
            var queueName = typeof (TMessage).Name.ToLower();
            var queue = _client.GetQueueReference(queueName);
            queue.CreateIfNotExists();

            return queue;
        }

    }
}