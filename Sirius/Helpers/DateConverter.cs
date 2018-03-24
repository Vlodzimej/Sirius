using System;
using System.Globalization;

namespace Sirius.Helpers
{
    static public class DateConverter
    {
        static public string ConvertToStandardString(DateTime value)
        {
            return value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
    }
}
