namespace MarsParcelTracking.Domain
{
    public class Parcel
    {
        public long Id { get; set; }
        public string? Barcode { get; set; }

        private EnumParcelStatus _status;
        public EnumParcelStatus Status
        {
            get { return _status; }
            set
            {
                if (value != _status)
                {
                    History.Add(new ParcelTransition() { ParcelId = Id, Status = value, Timestamp = DateTime.UtcNow });
                    _status = value;
                }
            }
        }
        public string? Sender { get; set; }
        public string? Recipient { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public EnumDeliveryService DeliveryService { get; set; }
        public string? Contents { get; set; }
        public DateTime? LaunchDate { get; set; }
        public int? EtaDays { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public List<ParcelTransition> History { get; set; }

        public Parcel()
        {
            _status = EnumParcelStatus.Initial;
            History = new List<ParcelTransition>();
        }
    }
}
