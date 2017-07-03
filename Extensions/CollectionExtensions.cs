using System.Collections.Generic;

namespace Helper.Extensions
{
    public static class CollectionExtensions
    {
        public static ICollection<T> AddRange<T>(this ICollection<T> value, IEnumerable<T> items)
        {
            items.Each(value.Add);

            return value;
        }
    }
}
