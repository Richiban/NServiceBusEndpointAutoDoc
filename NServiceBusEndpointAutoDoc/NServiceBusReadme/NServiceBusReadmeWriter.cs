using System.Collections.Generic;

namespace NServiceBusEndpointAutoDoc
{
    internal class NServiceBusReadmeWriter
    {
        private readonly MarkdownBuilder _markdownBuilder = new MarkdownBuilder();

        public NServiceBusReadmeWriter Write(NServiceBusReadmeModel model)
        {
            _markdownBuilder.AppendHeading("Handled messages");

            foreach (var entry in model.ReadmeEntries)
                WriteEntry(entry);

            return this;
        }

        private NServiceBusReadmeWriter WriteEntry(ReadmeEntry readmeEntry)
        {
            _markdownBuilder.AppendHeading(readmeEntry.ParameterTypeName, level: 2);
            _markdownBuilder.AppendBlockQuote(readmeEntry.ParameterTypeComments);
            _markdownBuilder.AppendTable(readmeEntry.ParameterPropertyInfos);
            _markdownBuilder.AppendParagraph(readmeEntry.EnclosingTypeComments);
            _markdownBuilder.AppendParagraph(readmeEntry.MethodComments);

            return this;
        }

        public override string ToString() => _markdownBuilder.ToString();
    }
}