using System.Globalization;

namespace MarsParcelTracking.Application
{
    public class Util
    {
        internal const string ISO8601PATTERN = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public static string? UTCDateToString(DateTime? date)
        {
            string? answer = null;
            if (date != null)
                answer = date.Value.ToUniversalTime().ToString(ISO8601PATTERN);
            return answer;
        }

        public static DateTime? StringToUTCDate(string? date)
        {
            DateTime? answer = null;
            if (date != null)
                answer = DateTime.ParseExact(date, ISO8601PATTERN, CultureInfo.InvariantCulture
                    , DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            return answer;
        }
    }
}
