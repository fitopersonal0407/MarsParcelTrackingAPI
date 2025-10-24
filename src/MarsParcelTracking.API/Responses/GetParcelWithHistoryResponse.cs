namespace MarsParcelTracking.API.Responses
{
    public class GetParcelWithHistoryResponse : GetParcelResponse
    {
        public List<GetParcelTransitionResponse> History { get; set; }
    }
}