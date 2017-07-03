using System;

namespace Helper.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal Round(this decimal value, RoundToPosition position)
        {
            var roundTo = Convert.ToInt32(position);

            return (int)(((double)value + (0.5 * roundTo)) / roundTo) * roundTo;
        }
    }
}
