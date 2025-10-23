namespace MarsParcelTracking.Domain
{
    public class Parcel
    {
        public long Id { get; set; }
        public string? Barcode { get; set; }
        public ParcelStatus Status { get; set; }
        public string? Sender { get; set; }
        public string? Recipient { get; set; }
        public DeliveryService DeliveryService { get; set; }
        public string? Contents { get; set; }
        public DateTime? LaunchDate { get; set; }
        public int? EtaDays { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
    }
}
