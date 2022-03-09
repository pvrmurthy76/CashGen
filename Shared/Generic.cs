using System;
using System.Text.RegularExpressions;

namespace CashGen.Shared
{
    internal class Generic
    {
        public bool isValidGuid(string guidstring)
        {
            Regex regex = new Regex("^(\\{){0,1}[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}(\\}){0,1}$", RegexOptions.Compiled);
            try
            {
                return regex.IsMatch(guidstring);
            }
            catch
            {
                return false;
            }
        }

        public DateTime ResetTimeToStartOfDay(DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, 0);

        public DateTime ResetTimeToEndOfDay(DateTime dateTime) => new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59, 999);
    }
}
