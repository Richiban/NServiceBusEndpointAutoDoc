using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NServiceBusEndpointAutoDoc
{
    class AssemblyCommentsCollection
    {
        private readonly XDocument _xmlComments;

        internal AssemblyCommentsCollection(XDocument xmlComments)
        {
            _xmlComments = xmlComments;
        }

        public string GetComments(Type type)
        {
            var typeName = type.FullName;

            return
                _xmlComments.XPathSelectElement(
                    $"//member[@name=\"T:{typeName}\"]")?.Value?.Trim();
        }

        public string GetComments(MethodInfo method)
        {
            var declaringTypeName = method.DeclaringType.FullName;
            var parameterTypesString = String.Join(",", method.GetParameters().Select(x => x.ParameterType));

            return
                _xmlComments.XPathSelectElement(
                    $"//member[@name=\"M:{declaringTypeName}.{method.Name}({parameterTypesString})\"]")?.Value?.Trim();
        }

        public IEnumerable<(PropertyInfo, string)> GetPropertyComments(Type type)
        {
            return
                type
                .GetProperties()
                .Select(p => (p, ""));
        }
    }
}
