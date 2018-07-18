using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NServiceBusEndpointAutoDoc
{
    class NServiceBusReadmeBuilder
    {
        private readonly IEnumerable<MethodInfo> _methods;

        private NServiceBusReadmeBuilder(IEnumerable<MethodInfo> methods)
        {
            _methods = methods;
        }

        public static NServiceBusReadmeBuilder LoadFrom(string assemblyPath)
        {
            var nsbAssemblyPath = GuessNsbAssemblyPath(assemblyPath);
            var nsbAssembly = Assembly.LoadFrom(nsbAssemblyPath);

            var messageHandlerInterface = nsbAssembly.GetType("NServiceBus.IHandleMessages`1");

            var handleMethods = Assembly.LoadFrom(assemblyPath)
                    .GetMethodsImplementing(messageHandlerInterface);

            return new NServiceBusReadmeBuilder(handleMethods);
        }

        private static string GuessNsbAssemblyPath(string assemblyPath)
        {
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            return Path.Combine(assemblyDirectory, "NServiceBus.Core.dll");
        }

        internal void WriteInfoTo(ReadmeBuilder readmeBuilder)
        {
            var readmeEntries = _methods.Select(CreateReadmeEntry);

            readmeBuilder.PrintEntries(readmeEntries);
        }

        ReadmeEntry CreateReadmeEntry(MethodInfo handleMethod)
        {
            var handlerType = handleMethod.DeclaringType;
            var handlerAssembly = handlerType.Assembly;
            var messageType = handleMethod.GetParameters().Single().ParameterType;
            var messageAssembly = messageType.Assembly;

            var handlerAssemblyComments = AssemblyCommentsCollectionFactory.Create(handlerAssembly);
            var messageAssemblyComments = AssemblyCommentsCollectionFactory.Create(messageAssembly);

            var messageParameters =
                messageAssemblyComments
                .GetPropertyComments(messageType)
                .Select(pc =>
                    new ParameterPropertyInfo
                    {
                        Field = pc.property.Name,
                        Type = pc.property.PropertyType.Name,
                        Comment = pc.comment
                    }
                )
                .ToList();

            return new ReadmeEntry
            {
                EnclosingTypeComments = handlerAssemblyComments.GetComments(handlerType),
                MethodComments = handlerAssemblyComments.GetComments(handleMethod),
                ParameterTypeComments = messageType.FullName,
                ParameterPropertyInfos = messageParameters
            };
        }
    }
}