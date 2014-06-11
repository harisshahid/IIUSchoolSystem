using System;
using System.Globalization;
using System.Linq;

namespace IIUSchoolSystem.Core.Helpers
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt)  //"this DateTime dt" for extenstion method instead of "DateTime dt"
        {
            int diff = dt.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        //public static DateTime AbsoluteEnd(this DateTime dateTime)  // Extension method
        //{
        //    return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1);
        //}

        public static DateTime AbsoluteEnd(DateTime dateTime) //For same day end
        {
            return dateTime.Date.AddDays(1).AddTicks(-1);
        }

        public static DateTime EndOfWeek(this DateTime dateTime) // for end of week
        {
            DateTime start = StartOfWeek(dateTime);

            return start.AddDays(7).AddTicks(-1);
        }

        public static int GetWorkingDays(DateTime from, DateTime to)
        {
            var dayDifference = (int)to.Subtract(from).TotalDays;
            return Enumerable
                .Range(1, dayDifference)
                .Select(x => from.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }

        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }
    }
}
