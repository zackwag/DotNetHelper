using System.Collections.Generic;

namespace Helper.Extensions
{
    public static class DictionaryExtensions
    {
        public static SortedDictionary<TK, TV> ToSortedDictionary<TK, TV>(this Dictionary<TK, TV> value)
        {
            return new SortedDictionary<TK, TV>(value);
        }
    }
}
