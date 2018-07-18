using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusEndpointAutoDoc
{
    public static class StringBuilderExtensions
    {
        public static void AppendTable<T>(this StringBuilder sb, IReadOnlyCollection<T> items)
        {
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
                sb.Append("| ");
                sb.Append(h);
                sb.Append(' ', maxLengths[i] - h.Length);
                sb.Append(" ");
            }

            sb.AppendLine("|");

            foreach (var l in maxLengths)
            {
                sb.Append("| ");
                sb.Append('-', l);
                sb.Append(" ");
            }

            sb.AppendLine("|");

            foreach (var y in Enumerable.Range(0, items.Count))
            {
                foreach (var x in Enumerable.Range(0, properties.Length))
                {
                    var val = values[x, y];

                    sb.Append("| ");
                    sb.Append(val);
                    sb.Append(' ', maxLengths[x] - val.Length);
                    sb.Append(" ");
                }

                sb.AppendLine("|");
            }
        }
    }
}
