using System;
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

            var nsbBuilder = NServiceBusReadmeBuilder.LoadFrom(assemblyPath);

            var readmeBuilder = new ReadmeBuilder();

            nsbBuilder.WriteInfoTo(readmeBuilder);

            Console.WriteLine(readmeBuilder.ToString());

            Console.ReadLine();
        }
    }
}