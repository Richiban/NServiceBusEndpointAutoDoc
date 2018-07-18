using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NServiceBusEndpointAutoDoc
{
    static class AssemblyExtensions
    {
        public static IEnumerable<MethodInfo> GetMethodsImplementing(this Assembly assembly, Type interfaceType) =>
            assembly
                .GetTypes()
                .SelectMany(t => t.GetMethodsImplementing(interfaceType));
    }
}