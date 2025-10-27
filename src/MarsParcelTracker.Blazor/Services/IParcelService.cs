using MarsParcelTracker.Blazor.Models;

namespace MarsParcelTracker.Blazor.Services
{
    public interface IParcelService
    {
        Task<List<GetParcelResponse>> GetParcels();
    }
}
