using System.Collections.Generic;

namespace NServiceBusEndpointAutoDoc
{
    static class EnumerableExtensions
    {
        public static IEnumerable<(T item, int index)> Indexed<T>(this IEnumerable<T> source)
        {
            var i = 0;

            foreach(var item in source)
            {
                yield return (item, i++);
            }
        }
    }
}
