using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NServiceBusEndpointAutoDoc
{
    class NServiceBusReadmeModel
    {
        private NServiceBusReadmeModel(IReadOnlyCollection<ReadmeEntry> readmeEntries)
        {
            ReadmeEntries = readmeEntries;
        }

        public IReadOnlyCollection<ReadmeEntry> ReadmeEntries { get; }

        public static NServiceBusReadmeModel LoadFrom(string assemblyPath)
        {
            var nsbAssemblyPath = GuessNsbAssemblyPath(assemblyPath);
            var nsbAssembly = Assembly.LoadFrom(nsbAssemblyPath);

            var messageHandlerInterface = nsbAssembly.GetType("NServiceBus.IHandleMessages`1");

            var handleMethods = Assembly.LoadFrom(assemblyPath)
                    .GetMethodsImplementing(messageHandlerInterface);

            var readmeEntries = handleMethods.Select(CreateReadmeEntry).ToList().AsReadOnly();

            return new NServiceBusReadmeModel(readmeEntries);
        }

        private static string GuessNsbAssemblyPath(string assemblyPath)
        {
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            return Path.Combine(assemblyDirectory, "NServiceBus.Core.dll");
        }

        private static ReadmeEntry CreateReadmeEntry(MethodInfo handleMethod)
        {
            var handlerType = handleMethod.DeclaringType;
            var handlerAssembly = handlerType.Assembly;
            var messageType = handleMethod.GetParameters().Single().ParameterType;
            var messageAssembly = messageType.Assembly;

            var handlerAssemblyComments = AssemblyCommentsCollectionFactory.Create(handlerAssembly);
            var messageAssemblyComments = AssemblyCommentsCollectionFactory.Create(messageAssembly);

            var messageParameterInfos =
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

            var messageTypeComents = messageAssemblyComments.GetComments(messageType);

            return new ReadmeEntry
            {
                EnclosingTypeComments = handlerAssemblyComments.GetComments(handlerType),
                MethodComments = handlerAssemblyComments.GetComments(handleMethod),
                ParameterTypeName = messageType.FullName,
                ParameterTypeComments = messageTypeComents,
                ParameterPropertyInfos = messageParameterInfos
            };
        }
    }
}