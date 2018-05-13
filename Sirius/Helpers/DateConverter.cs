using System;
using System.Globalization;

namespace Sirius.Helpers
{
    static public class DateConverter
    {
        static public DateTime ConvertToRTS(DateTime dateTime)
        {
            DateTime utcDateTime = dateTime.ToUniversalTime();

            string nzTimeZoneKey = "Russian Standard Time";
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
