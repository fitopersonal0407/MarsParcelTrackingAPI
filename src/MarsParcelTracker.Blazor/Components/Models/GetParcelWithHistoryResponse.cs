namespace MarsParcelTracker.Blazor.Components.Models
{
    public class GetParcelWithHistoryResponse : GetParcelResponse
    {
        public List<GetParcelTransitionResponse> History { get; set; }
    }
}
