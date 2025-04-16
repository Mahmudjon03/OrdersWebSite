using System;

namespace RestaurantLibrary.Helpers
{
    public static class DateTimeHelper
    {
        // Метод для получения текущей даты (без времени) в формате MySQL DATE
        public static DateTime GetCurrentDate()
        {
            return DateTime.Today;
        }

        // Метод для получения текущей даты и времени в формате MySQL DATETIME
        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public static string GetCurrentDateString()
        {
            return DateTime.Today.ToString("yyyy-MM-dd"); 
        }

        public static string GetCurrentDateTimeString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm"); 
        }
    }
}
