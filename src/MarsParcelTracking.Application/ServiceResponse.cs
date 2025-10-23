namespace MarsParcelTracking.Application
{
    public class ServiceResponse<T>
    {
        public bool Success { get { return Response == ServiceResponseCode.OK; } }
        public ServiceResponseCode Response { get; set; } = ServiceResponseCode.OK;
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public enum ServiceResponseCode { OK, BarcodeInvalid, UnexpectedError }
}
