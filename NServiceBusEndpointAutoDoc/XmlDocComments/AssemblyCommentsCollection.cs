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

        internal AssemblyCommentsCollection()
        {
            _xmlComments = new XDocument();
        }

        public string GetComments(Type type) => 
            _xmlComments.XPathSelectElement(
                $"//member[@name=\"T:{type.FullName}\"]")?.Value?.Trim();

        public string GetComments(MethodInfo method)
        {
            var declaringTypeName = method.DeclaringType.FullName;
            var parameterTypesString = String.Join(",", method.GetParameters().Select(x => x.ParameterType));

            return
                _xmlComments.XPathSelectElement(
                    $"//member[@name=\"M:{declaringTypeName}.{method.Name}({parameterTypesString})\"]")?.Value?.Trim();
        }

        public IEnumerable<(PropertyInfo property, string comment)> GetPropertyComments(Type type) => 
            type
                .GetProperties()
                .Select(p => (p, GetPropertyComment(p)));

        private string GetPropertyComment(PropertyInfo p) =>
            _xmlComments.XPathSelectElement(
                $"//member[@name=\"P:{p.DeclaringType.FullName}.{p.Name}\"]")?.Value?.Trim();
    }
}
