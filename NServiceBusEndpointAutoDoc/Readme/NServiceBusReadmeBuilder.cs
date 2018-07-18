using System.Collections.Generic;
using System.Text;

namespace NServiceBusEndpointAutoDoc
{
    class ReadmeBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        internal void PrintEntries(IEnumerable<ReadmeEntry> readmeEntries)
        {
            foreach (var entry in readmeEntries)
                PrintEntry(entry);
        }

        internal void PrintEntry(ReadmeEntry readmeEntry)
        {
            _stringBuilder.AppendLine($"## {readmeEntry.MessageType}");
            _stringBuilder.AppendLine("");
            _stringBuilder.AppendLine($"{readmeEntry.HandlerTypeComments}");
            _stringBuilder.AppendLine("");
            _stringBuilder.AppendLine($"{readmeEntry.HandlerMethodComments}");
            _stringBuilder.AppendLine("");

            _stringBuilder.AppendTable(readmeEntry.MessageParameters);
            _stringBuilder.AppendLine("");
            _stringBuilder.AppendLine("");
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}