using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Azure.WebJobs;
using WebJob.MessageBus.Dispatch;

namespace WebJob.MessageBus
{
    public class FunctionsTypeGenerator
    {
        private readonly Assembly _assemblyWithMessages;
        private readonly Type _messageBaseType;
        private IEnumerable<string> _messages;
        private IEnumerable<string> _messageNamespaces;

        public FunctionsTypeGenerator(Assembly messageAssembly)
        {
            _messageBaseType = typeof (Message);
            _assemblyWithMessages = messageAssembly;
        }

        public Type GenerateType()
        {
            Trace.WriteLine($"Generating WebJob functions type using {_assemblyWithMessages.FullName}");
            var template = new FunctionsTemplate();
            FindMessageTypesMatching(_messageBaseType);
            FillTemplate(template);

            var result = CompileScript(template.ToString());

            foreach (var error in result.Errors) Trace.WriteLine(error);

            return  result.CompiledAssembly?.DefinedTypes.First();
        }

        private void FindMessageTypesMatching(Type messageBaseType)
        {
            var messages = (from type in _assemblyWithMessages.ExportedTypes
                            where type.BaseType == messageBaseType
                            select new { type.Namespace, type.Name })
                .ToList();

            _messages = messages.Select(message => message.Name).ToList();
            _messageNamespaces = messages.Select(message => message.Namespace).Distinct().ToList();
        }

        private void FillTemplate(FunctionsTemplate template)
        {
            _messageNamespaces.ForEachDo(template.AddMessageNamespace);
            _messages.ForEachDo(template.AddTriggerMethod);
        }

        private CompilerResults CompileScript(string source)
        {
            var parms = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                IncludeDebugInformation = false
            };
            
            parms.ReferencedAssemblies.Add(_assemblyWithMessages.Location);
            parms.ReferencedAssemblies.Add(typeof (FunctionsBase).Assembly.Location);
            parms.ReferencedAssemblies.Add(typeof (MessageDispatcher).Assembly.Location);
            parms.ReferencedAssemblies.Add(Path.Combine(typeof(QueueTriggerAttribute).Assembly.Location));

            var compiler = CodeDomProvider.CreateProvider("CSharp");

            return compiler.CompileAssemblyFromSource(parms, source);
        }

    }
}