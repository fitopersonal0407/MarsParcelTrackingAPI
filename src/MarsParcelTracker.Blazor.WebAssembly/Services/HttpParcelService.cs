using MarsParcelTracker.Blazor.WebAssembly.Models;
using System.Net.Http.Json;

namespace MarsParcelTracker.Blazor.WebAssembly.Services
{
    public class HttpParcelService : IParcelService
    {
        private readonly IMessageService messageService;
        private readonly HttpClient httpClient;
        public HttpParcelService(IMessageService messageService, HttpClient httpClient)
        {
            this.messageService = messageService;
            this.httpClient = httpClient;
        }
        public async Task<List<GetParcelResponse>> GetParcels()
        {
            return await httpClient.GetFromJsonAsync<List<GetParcelResponse>>("/api/parcels/");
        }
    }

}
