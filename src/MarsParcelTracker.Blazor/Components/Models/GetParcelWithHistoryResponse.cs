namespace MarsParcelTracker.Blazor.Components.Models
{
    public class GetParcelWithHistoryResponse : GetParcelResponse
    {
        public required List<GetParcelTransitionResponse> History { get; set; }
    }
}
