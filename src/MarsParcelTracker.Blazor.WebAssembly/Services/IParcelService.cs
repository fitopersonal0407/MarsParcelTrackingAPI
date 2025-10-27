using MarsParcelTracker.Blazor.WebAssembly.Models;

namespace MarsParcelTracker.Blazor.WebAssembly.Services
{
    public interface IParcelService
    {
        Task<List<GetParcelResponse>> GetParcels();
    }
}
