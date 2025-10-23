namespace MarsParcelTracking.Application
{
    internal class Util
    {
        public static string? UTCDateToString(DateTime? date)
        {
            string? answer = null;
            if (date != null)
                answer = date.Value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            return answer;
        }
    }
}
