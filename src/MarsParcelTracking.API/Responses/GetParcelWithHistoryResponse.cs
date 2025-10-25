namespace MarsParcelTracking.API.Responses
{
    public class GetParcelWithHistoryResponse : GetParcelResponse
    {
        public required List<GetParcelTransitionResponse> History { get; set; }
    }
}