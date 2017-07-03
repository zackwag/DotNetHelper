using System.Collections.Generic;

namespace Helper.Extensions
{
    public static class QueueExtensions
    {
        public static IEnumerable<T> TakeAndRemove<T>(this Queue<T> value, int count)
        {
            for (var i = 0; i < count && value.Count > 0; i++)
                yield return value.Dequeue();
        }
    }
}
