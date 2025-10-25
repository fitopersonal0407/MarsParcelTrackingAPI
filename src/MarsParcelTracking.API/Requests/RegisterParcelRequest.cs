namespace MarsParcelTracking.API
{
    public class RegisterParcelRequest
    {
        public required string Barcode { get; set; }
        public required string Sender { get; set; }
        public required string Recipient { get; set; }
        public required string DeliveryService { get; set; }
        public required string Contents { get; set; }
    }
}
