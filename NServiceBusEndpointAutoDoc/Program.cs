using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NServiceBusEndpointAutoDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            var assemblyPath = @"C:\Source\Asos.Marketplace\Commerce.Basket.Messaging\Messaging\bin\Debug\Asos.Marketplace.Commerce.Basket.Messaging.dll";
            var nsb = Assembly.LoadFrom(@"C:\Source\Asos.Marketplace\Commerce.Basket.Messaging\Messaging\bin\Debug\NServiceBus.Core.dll");

            var xmlCommentsFilePath = assemblyPath.Replace(".dll", ".xml");

            var xmlComments = XDocument.Load(xmlCommentsFilePath);

            var messageHandlerInterface = nsb.GetType("NServiceBus.IHandleMessages`1");

            var readmeEntries =
                Assembly.LoadFrom(assemblyPath)
                    .GetTypes()
                    .SelectMany(t => t.GetMethodsImplementing(messageHandlerInterface))
                    .Select(CreateReadmeEntry)
                    .ToArray();

            var sb = new StringBuilder();

            foreach (var item in readmeEntries)
                PrintEntry(item, sb);

            Console.WriteLine(sb.ToString());

            Console.ReadLine();

            ReadmeEntry CreateReadmeEntry(MethodInfo methodInfo)
            {
                var declaringTypeName = methodInfo.DeclaringType.FullName;
                var messageType = methodInfo.GetParameters().Single().ParameterType;
                var messageTypeName = messageType.FullName;

                var typeComments =
                    xmlComments.XPathSelectElement(
                        $"//member[@name=\"T:{declaringTypeName}\"]")?.Value?.Trim();

                var methodComments =
                    xmlComments.XPathSelectElement(
                        $"//member[@name=\"M:{declaringTypeName}.{methodInfo.Name}({messageTypeName})\"]")?.Value?.Trim();

                var messageParameters =
                    messageType
                    .GetProperties()
                    .Select(p => new ReadmeMessageParameter { Field = p.Name, Type = p.PropertyType.Name, Comment = "" })
                    .ToList();

                return new ReadmeEntry
                {
                    HandlerTypeComments = typeComments,
                    HandlerMethodComments = methodComments,
                    MessageType = messageTypeName,
                    MessageParameters = messageParameters
                };
            }
        }

        private static void PrintEntry(ReadmeEntry r, StringBuilder sb)
        {
            sb.AppendLine($"## {r.MessageType}");
            sb.AppendLine("");
            sb.AppendLine($"{r.HandlerTypeComments}");
            sb.AppendLine("");
            sb.AppendLine($"{r.HandlerMethodComments}");
            sb.AppendLine("");

            sb.AppendTable(r.MessageParameters);
            sb.AppendLine("");
            sb.AppendLine("");
        }
    }
}
