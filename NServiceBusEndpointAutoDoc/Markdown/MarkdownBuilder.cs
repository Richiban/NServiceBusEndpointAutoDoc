using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusEndpointAutoDoc
{
    class MarkdownBuilder
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void AppendTable<T>(IReadOnlyCollection<T> items)
        {
            if (items.Count == 0)
                return;

            MaybeAddBlankLine();

            var properties = typeof(T).GetProperties();
            var maxLengths = new int[properties.Length];

            var headings = new string[properties.Length];
            var values = new string[properties.Length, items.Count];

            foreach (var (p, i) in properties.Indexed())
            {
                headings[i] = p.Name;
                maxLengths[i] = Math.Max(maxLengths[i], p.Name.Length);
            }

            foreach (var (p, x) in properties.Indexed())
            {
                foreach (var (v, y) in items.Indexed())
                {
                    var val = p.GetValue(v)?.ToString() ?? "";
                    values[x, y] = $"{val}";
                    maxLengths[x] = Math.Max(maxLengths[x], val.Length);
                }
            }

            foreach (var (h, i) in headings.Indexed())
            {
                _stringBuilder.Append("| ");
                _stringBuilder.Append(h);
                _stringBuilder.Append(' ', maxLengths[i] - h.Length);
                _stringBuilder.Append(" ");
            }
            
            _stringBuilder.AppendLine("|");

            foreach (var l in maxLengths)
            {
                _stringBuilder.Append("| ");
                _stringBuilder.Append('-', l);
                _stringBuilder.Append(" ");
            }

            _stringBuilder.Append("|");

            foreach (var y in Enumerable.Range(0, items.Count))
            {
                _stringBuilder.AppendLine();

                foreach (var x in Enumerable.Range(0, properties.Length))
                {
                    var val = values[x, y];

                    _stringBuilder.Append("| ");
                    _stringBuilder.Append(val);
                    _stringBuilder.Append(' ', maxLengths[x] - val.Length);
                    _stringBuilder.Append(" ");
                }

                _stringBuilder.Append("|");
            }
        }

        public void AppendBlockQuote(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            MaybeAddBlankLine();

            var prefixString = "> ";

            _stringBuilder.Append(prefixString);
            _stringBuilder.Append(content?.Replace(Environment.NewLine, $"{Environment.NewLine}{prefixString}"));
        }

        public void AppendParagraph(string content)
        {
            if (string.IsNullOrEmpty(content))
                return;

            MaybeAddBlankLine();

            _stringBuilder.Append(content);
        }

        public void AppendHeading(string heading, int level = 1)
        {
            if (string.IsNullOrEmpty(heading))
                return;

            MaybeAddBlankLine();

            _stringBuilder.Append('#', level);
            _stringBuilder.Append(' ');
            _stringBuilder.Append(heading);
        }

        private void MaybeAddBlankLine()
        {
            if (_stringBuilder.Length == 0)
                return;

            _stringBuilder.AppendLine();
            _stringBuilder.AppendLine();
        }

        public override string ToString() => _stringBuilder.ToString();
    }
}
