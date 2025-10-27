namespace MarsParcelTracker.Blazor.WebAssembly.Models
{
    public class GetParcelWithHistoryResponse : GetParcelResponse
    {
        public List<GetParcelTransitionResponse> History { get; set; }
    }
}
