namespace MarsParcelTracking.Domain
{
    public class ParcelTransition
    {
        public long Id { get; set; }
        public EnumParcelStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
