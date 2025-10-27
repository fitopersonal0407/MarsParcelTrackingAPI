namespace MarsParcelTracker.Blazor.Models
{
    public class GetParcelWithHistoryResponse : GetParcelResponse
    {
        public List<GetParcelTransitionResponse> History { get; set; }
    }
}
