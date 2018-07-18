using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace NServiceBusEndpointAutoDoc
{
    static class AssemblyCommentsCollectionFactory
    {
        private static readonly ConcurrentDictionary<Assembly, AssemblyCommentsCollection> _cache = 
            new ConcurrentDictionary<Assembly, AssemblyCommentsCollection>();

        public static AssemblyCommentsCollection Create(Assembly assembly)
        {
            if (_cache.ContainsKey(assembly))
                return _cache[assembly];

            var newValue = LoadFrom(assembly);
            _cache[assembly] = newValue;

            return newValue;
        }

        private static AssemblyCommentsCollection LoadFrom(Assembly assembly)
        {
            var xmlCommentsFilePath = assembly.Location.Replace(".dll", ".xml");

            if (!File.Exists(xmlCommentsFilePath))
                return new AssemblyCommentsCollection();

            var xmlComments = XDocument.Load(xmlCommentsFilePath);

            return new AssemblyCommentsCollection(xmlComments);
        }
    }
}
