using System.Text;

namespace WebJob.MessageBus
{
    public class FunctionsTemplate
    {
        private const string Indention = "            ";
        private static readonly string TypeTemplate = $@"
        $messagesNamespace$
        using WebJob.MessageBus;
        using WebJob.MessageBus.Dispatch;
        using Microsoft.Azure.WebJobs;
        using System;
    
        public class Functions : FunctionsBase
        {{            
            public Functions(MessageDispatcher dispatcher) : base(dispatcher) {{ }}
            $functions$
        }}";

        private readonly StringBuilder _functionsBuilder = new StringBuilder();
        private readonly StringBuilder _namespaceBuilder = new StringBuilder();

        public void AddMessageNamespace(string @namespace)
        {
            _namespaceBuilder.AppendLine($"{Indention}using {@namespace};");
        }

        public void AddTriggerMethod(string messageName)
        {
            var queueName = messageName.ToLower();

            _functionsBuilder.AppendLine(
                $@"{Indention}public void Handle{messageName}Message([QueueTrigger(""{queueName}"")] {messageName} message) 
                             {{ Dispatcher.Dispatch(message); }}");
        }

        public override string ToString()
        {
            return TypeTemplate.Replace("$messagesNamespace$", _namespaceBuilder.ToString())
                               .Replace("$functions$", _functionsBuilder.ToString());

        }

    }
}