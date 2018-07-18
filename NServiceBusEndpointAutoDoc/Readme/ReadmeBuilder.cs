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
            _stringBuilder.AppendLine($"## {readmeEntry.ParameterTypeComments}");
            _stringBuilder.AppendLine("");
            _stringBuilder.AppendLine($"{readmeEntry.EnclosingTypeComments}");
            _stringBuilder.AppendLine("");
            _stringBuilder.AppendLine($"{readmeEntry.MethodComments}");
            _stringBuilder.AppendLine("");

            _stringBuilder.AppendTable(readmeEntry.ParameterPropertyInfos);
            _stringBuilder.AppendLine("");
            _stringBuilder.AppendLine("");
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}