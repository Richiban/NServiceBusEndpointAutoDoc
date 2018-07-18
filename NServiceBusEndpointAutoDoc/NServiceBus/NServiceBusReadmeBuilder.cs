using System.Collections.Generic;
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
            var nsbAssembly = Assembly.LoadFrom(@"C:\Source\Asos.Marketplace\Commerce.Basket.Messaging\Messaging\bin\Debug\NServiceBus.Core.dll");

            var messageHandlerInterface = nsbAssembly.GetType("NServiceBus.IHandleMessages`1");

            var handleMethods = Assembly.LoadFrom(assemblyPath)
                    .GetMethodsImplementing(messageHandlerInterface);

            return new NServiceBusReadmeBuilder(handleMethods);
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
                    new ReadmeMessageParameter
                    {
                        Field = pc.property.Name,
                        Type = pc.property.PropertyType.Name,
                        Comment = pc.comment
                    }
                )
                .ToList();

            return new ReadmeEntry
            {
                HandlerTypeComments = handlerAssemblyComments.GetComments(handlerType),
                HandlerMethodComments = handlerAssemblyComments.GetComments(handleMethod),
                MessageType = messageType.FullName,
                MessageParameters = messageParameters
            };
        }
    }
}