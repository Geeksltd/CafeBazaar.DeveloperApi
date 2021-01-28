namespace CafeBazaar.DeveloperApi
{
    using System;
    using Olive;

    static class DateTimeExtensions
    {
        public static DateTime ToDateTime(this TimeSpan @this) => LocalTime.Now.Add(@this);
    }
}
