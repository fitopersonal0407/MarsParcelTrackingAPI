using MarsParcelTracker.Blazor.WebAssembly.Models;

namespace MarsParcelTracker.Blazor.WebAssembly.Services
{
    public class ParcelService : IParcelService
    {
        private readonly IMessageService messageService;
        public ParcelService(IMessageService messageService) => this.messageService = messageService;
        List<GetParcelResponse> parcels { get; set; } = new List<GetParcelResponse>
        {
            new GetParcelResponse() { Barcode = "11", Status = "bla 11" },
            new GetParcelResponse() { Barcode = "22", Status = "bla 22" },
            new GetParcelResponse() { Barcode = "33", Status = "bla 33" },
        };

        public async Task<List<GetParcelResponse>> GetParcels()
        {
            messageService.Add("ParcelService: fetched parcels");
            return await Task.Run(() => { return parcels; });
        }
    }
}
