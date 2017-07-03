using System;

namespace Helper.Extensions
{
    public static class DateTimeExtensions
    {
        private const int FISCAL_YEAR_END_MONTH = 6;

        public static DateTime AddBusinessDays(this DateTime value, int days)
        {
            // start from a weekday
            while (value.IsWeekDay())
                value = value.AddDays(1);

            for (var i = 0; i < days; ++i)
            {
                value = value.AddDays(1);

                while (value.IsWeekDay())
                    value = value.AddDays(1);
            }

            return value;
        }

        #region 'Is' Methods
        public static bool IsWeekend(this DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Saturday || value.DayOfWeek == DayOfWeek.Sunday;
        }

        public static bool IsWeekDay(this DateTime value)
        {
            return !IsWeekend(value);
        }

        public static bool IsBetween(this DateTime value, DateTime startDate, DateTime endDate, bool compareTime = false)
        {
            return compareTime ? value >= startDate && value <= endDate : value.Date >= startDate.Date && value.Date <= endDate.Date;
        }
        #endregion

        #region 'Get' Methods
        public static DateTime GetFirstDayOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime GetLastDayOfMonth(this DateTime value)
        {
            return value.GetFirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        public static string GetFriendlyString(this DateTime value)
        {
            string formattedDate;

            if (value.Date == DateTime.Today)
                formattedDate = "Today";
            else if (value.Date == DateTime.Today.AddDays(-1))
                formattedDate = "Yesterday";
            else if (value.Date > DateTime.Today.AddDays(-6))
                // *** Show the Day of the week
                formattedDate = value.ToString("dddd");
            else
                formattedDate = value.ToString("MMMM dd, yyyy");

            //append the time portion to the output
            formattedDate += " @ " + value.ToString("t").ToLower();

            return formattedDate;
        }

        public static TimeSpan GetTimeElapsed(this DateTime value)
        {
            return DateTime.Now.Subtract(value);
        }

        public static int GetFiscalYear(this DateTime value)
        {
            var fyEnd = new DateTime(value.Year, FISCAL_YEAR_END_MONTH, 30);

            return value > fyEnd ? value.Year + 1 : value.Year;
        }

        public static string GetDaySuffix(this DateTime value)
        {
            switch (value.Day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        #endregion
    }
}
