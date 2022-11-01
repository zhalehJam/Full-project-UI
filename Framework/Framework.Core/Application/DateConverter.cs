using System;
using System.Globalization;

namespace Framework.Core.Application
{
    public static class DateConverter
    {
        public static DateTime PersianDateToGregorian(int year , int month , int day)
        {
            PersianCalendar pc = new PersianCalendar();
            return new DateTime(year, month, day, pc);
        }

    }
}
