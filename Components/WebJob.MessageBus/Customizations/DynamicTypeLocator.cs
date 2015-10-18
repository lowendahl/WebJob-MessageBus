using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Azure.WebJobs;


namespace WebJob.MessageBus
{
    public class DynamicTypeLocator : ITypeLocator
    {
        private readonly FunctionsTypeGenerator _generator;

        public DynamicTypeLocator(Assembly messageAssembly)
        {
            _generator = new FunctionsTypeGenerator(messageAssembly);
        }
        
        public IReadOnlyList<Type> GetTypes()
        {
            return new List<Type> { _generator.GenerateType() };
        }
    }
}