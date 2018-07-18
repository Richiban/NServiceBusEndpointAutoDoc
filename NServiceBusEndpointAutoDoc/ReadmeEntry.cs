using System.Collections.Generic;

namespace NServiceBusEndpointAutoDoc
{
    public class ReadmeEntry
    {
        public string MessageType { get; internal set; }
        public string HandlerTypeComments { get; internal set; }
        public string HandlerMethodComments { get; internal set; }
        public IReadOnlyList<ReadmeMessageParameter> MessageParameters { get; internal set; }
    }
}
