namespace MarsParcelTracking.API
{
    public class RegisterParcelRequest
    {
        public string? Barcode { get; set; }
        public string? Sender { get; set; }
        public string? Recipient { get; set; }
        public string DeliveryService { get; set; }
        public string? Contents { get; set; }
    }
}
