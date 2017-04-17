using System;

namespace QuakeViewer.Utils
{
    public class DateTimeUtils
    {

        public DateTime? GetDateFromString(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }
            DateTime convertTime;
            if (DateTime.TryParse(dateString, out convertTime))
            {
                return convertTime;
            }

            return null;
        }

        public static DateTime GetdayEndTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        }

        public static DateTime GetdayStartTime(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
    }
}