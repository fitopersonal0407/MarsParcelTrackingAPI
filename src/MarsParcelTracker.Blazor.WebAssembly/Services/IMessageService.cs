namespace MarsParcelTracker.Blazor.WebAssembly.Services
{
    public interface IMessageService
    {
        void Add(string message);
        void Clear();
        List<string> Messages { get; set; }
    }
}
