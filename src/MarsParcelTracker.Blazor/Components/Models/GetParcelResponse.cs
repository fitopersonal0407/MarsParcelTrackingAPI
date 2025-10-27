namespace MarsParcelTracker.Blazor.Components.Models
{
    public class GetParcelResponse
    {
        public  string Barcode { get; set; }
        public  string Status { get; set; }
        public  string Sender { get; set; }
        public  string Recipient { get; set; }
        public  string Origin { get; set; }
        public  string Destination { get; set; }
        public  string DeliveryService { get; set; }
        public  string Contents { get; set; }
        public  string LaunchDate { get; set; }
        public  int EtaDays { get; set; }
        public  string EstimatedArrivalDate { get; set; }
    }
}
