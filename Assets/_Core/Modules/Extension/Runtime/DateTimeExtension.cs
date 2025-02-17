using System;

namespace Kelsey.Extension
{
    public static class DateTimeExtension
    {
        public static DateTime GetToday()
        {
            return DateTime.Now.Date;
        }

        public static string ToDateTimeString(this TimeSpan time)
        {
            if (time > TimeSpan.FromDays(1))
            {
                return time.Days + "d " + time.Hours + "h";
            }
            else
            {
                return time.ToString(@"hh\:mm\:ss");
            }
        }
    }
}