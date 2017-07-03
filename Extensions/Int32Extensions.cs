using System;

namespace Helper.Extensions
{
    public static class Int32Extensions
    {
        public static double Round(this int value, RoundToPosition position)
        {
            var roundTo = Convert.ToInt32(position);

            return (value + 0.5 * roundTo) / roundTo * roundTo;
        }

        public static bool IsPrime(this int value)
        {
            if (value % 2 == 0)
                return value == 2;

            var sqrt = (int) Math.Sqrt(value);

            for (var t = 3; t <= sqrt; t = t + 2)
            {
                if (value % t == 0)
                    return false;
            }

            return value != 1;
        }

        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        public static bool IsOdd(this int value)
        {
            return !value.IsEven();
        }

        public static bool IsBetween(this int value, int start, int end)
        {
            return value > start && value < end;
        }
    }
}
