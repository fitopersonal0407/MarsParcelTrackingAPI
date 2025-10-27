using MarsParcelTracker.Blazor.Models;

namespace MarsParcelTracker.Blazor.Services
{
    public class ParcelService : IParcelService
    {
        List<GetParcelResponse> parcels { get; set; } = new List<GetParcelResponse>
        {
            new GetParcelResponse() { Barcode = "11", Status = "bla 11" },
            new GetParcelResponse() { Barcode = "22", Status = "bla 22" },
        };

        public async Task<List<GetParcelResponse>> GetParcels()
        {
            return await Task.Run(() => { return parcels; });
        }
    }
}
