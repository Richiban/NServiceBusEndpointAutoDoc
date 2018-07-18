using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NServiceBusEndpointAutoDoc
{
    public static class TypeExtensions
    {
        public static IEnumerable<MethodInfo> GetMethodsImplementing(this Type concreteType, Type interfaceType) =>
            concreteType
                .GetInterfaces()
                .SelectMany(i =>
                {
                    if (i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType)
                        return concreteType.GetInterfaceMap(i).TargetMethods;
                    else return new MethodInfo[0];
                });
    }
}
