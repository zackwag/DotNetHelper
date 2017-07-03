using System;

namespace Helper.Extensions
{
    public static class DoubleExtensions
    {
        public static double Round(this double value, RoundToPosition position)
        {
            var roundTo = Convert.ToInt32(position);

            return (int)((value + (0.5 * roundTo)) / roundTo) * roundTo;
        }
    }
}
