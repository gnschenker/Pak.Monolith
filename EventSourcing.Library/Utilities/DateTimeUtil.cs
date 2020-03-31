using System;

namespace EventSourcing.Library.Utilities
{
    public static class DateTimeUtil
    {
        private static Func<DateTime> _now = () => DateTime.Now;
        public static DateTime Now => _now();
        public static void MockNow(DateTime now)
        {
            _now = () => now;
        }
        public static void Reset()
        {
            _now = () => DateTime.Now;
        }
    }
}
