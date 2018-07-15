using System;
using System.Globalization;

namespace Sirius.Helpers
{
    static public class DateConverter
    {
        static public DateTime ConvertToRTS(DateTime dateTime)
        {
            DateTime utcDateTime = dateTime.ToUniversalTime();
            // Зона 'Russian Standard Time' не работает и вызывает исключение в Centos, так как отсутствует
            string nzTimeZoneKey = "Europe/Moscow";
            TimeZoneInfo nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById(nzTimeZoneKey);
            DateTime nzDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, nzTimeZone);
            return nzDateTime;
        }
        static public string ConvertToStandardString(DateTime value)
        {
            return value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
    }
}
