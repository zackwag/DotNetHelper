using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Helper.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool HasItems(this IEnumerable value)
        {
            if (value.IsNull())
                return false;

            try
            {
                var enumerator = value.GetEnumerator();

                if (enumerator.HasValue() && enumerator.MoveNext())
                    return true;
            }
            catch(Exception)
            {
                //fail silently
            }

            return false;
        }

        public static IEnumerable<T> Each<T>(this IEnumerable<T> value, Action<T> action)
        {
            foreach (var val in value)
            {
                action(val);
            }

            return value;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> value)
        {
            var r = new Random((int) DateTime.Now.Ticks);

            return value.Select(x => new { Number = r.Next(), Item = x }).OrderBy(x => x.Number).Select(x => x.Item).ToList();
        }

        public static bool None<T>(this IEnumerable<T> value)
        {
            return value.Any() == false;
        }

        public static bool None<T>(this IEnumerable<T> value, Func<T, bool> query)
        {
            return value.Any(query) == false;
        }
    }
}
