using System.Collections.Generic;

namespace NServiceBusEndpointAutoDoc
{
    class ReadmeEntry
    {
        public string ParameterTypeComments { get; internal set; }
        public string EnclosingTypeComments { get; internal set; }
        public string MethodComments { get; internal set; }
        public IReadOnlyList<ParameterPropertyInfo> ParameterPropertyInfos { get; internal set; }
    }
}
