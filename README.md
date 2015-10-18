#WebJob as a MessageBus
This is example code to illustrate message bus pattern using Azure. It turns an Azure WebJob into a dynamic MessageBus that 
adheres to the "Convention over Configuration" and Open/Closed-principles. In essence, this code demonstrates how to automatically register 
new `QueueTriggers` with the WebJob based on available messages.

By using this solution, listening to a new message on a new queue is as simple as implementing a new message an a new handler:

```
    public class NewImageAdded : Message
    {
        public string ImageName { get; set; }
    }
```
```
    public class NewImageAddedHandler : IHandle<NewImageAdded>
    {
        public void Handle(NewImageAdded message)
        {
            Console.WriteLine($"Handling image: {message.ImageName}");
        }
    }
```

All registration with the WebJob, the dispatcher and the Unity container will be automatically handled for you.


###Features
To achieve the Message bus pattern, the solution includes:

- A custom `TypeLocator` that creates and registers `QueueTrigger` methods automatically
- A custom `JoabActivator` that uses a unity container to resolve the functions class
- A `MessageBus` that dispatches messages to all `IHandle<T>` listening to a specific message
- An example project. 

###Get started
To get started with this sample code you will need an Azure Storage account and configure the WebJob. For the MesageBus you will need to add 
    the storage keys in the `WebJob.MessageBus` App.Config:

```
  <connectionStrings>
    <add name="AzureWebJobsStorage" connectionString="[AZURE STORAGE ACCOUNT]" />
    <add name="AzureWebJobsDashboard" connectionString="[AZURE STORAGE ACCOUNT]" />
  </connectionStrings>
```
  For the example client you will need to add the Azure Storage Account to the following ConnectionString:
```
  <connectionStrings>
    <add name="BlobStorageConnectionString" connectionString="[AZURE STORAGE ACCOUNT]" />
  </connectionStrings>
```

[Here is the steps to configure WebJobs](https://azure.microsoft.com/en-us/documentation/articles/websites-dotnet-webjobs-sdk-get-started/#storage)
     | 
[More information on Azure connection strings](https://azure.microsoft.com/en-us/documentation/articles/storage-configure-connection-string/)

###Solution explained
The solution is based on two main components:

- WebJob application (`WebJob.MessageBus` project)
- Message Dispatcher (`WebJob.MessageBus.Dispatcher` project)

####WebJob.MessageBus
This is a standard Console application that register a WebJob `JobHost`. It configures the host using 
    a custom 'TypeLocator' that uses the `CodeDomProvider` to dynamically compile a Functions class
    containing trigger methods for each `Message` type found in the provided message assembly. 

It uses the lower case message name as the queue name in the registration. 

####WebJob.Dispatcher
This component will automatically register any number of handlers for each message type provided and 
    when recieving a message on a queue will ensure to dispatch the recieved message to all listening
    handlers. 

 As an example; the message `NewImageAdded` could have three handlers: 

- CreateThumbnail
- ExtractExIFMetadata
- PublishToCDN

 Which would all be executed when a `NewImageAdded` messages is posted on the `newimageadded` Azure Storage Queue.

####Creating a new Message
Any new message the solutions should listen to needs to inherit the `WebJob.MessageBus.Dispatch.Message` class from the `WebJob.MessageBus.Dispatch` project.

####Creating a new handler
Any new handler for a message need to implement the `WebJob.MessageBus.Dispatch.Message` class from the `WebJob.MessageBus.Dispatch.IHandle<T>`class from the `WebJob.MessageBus.Dispatch` project.

