using MarsParcelTracking.API;
using MarsParcelTracking.API.Responses;
using System.Net.Http.Json;

namespace MarsParcelTracking.IntegrationTest
{
    public class ParcelIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public ParcelIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterParcel_ValidRequest_ReturnsSuccess()
        {
            var registerParcelRequest = new RegisterParcelRequest
            {
                Barcode = "RMARS1234567890123456789A",
                Sender = "Cuco Malanga",
                Recipient = "Conde Monte Cristo",
                DeliveryService = "Standard",
                Contents = "Signed C# language specification and a birthday card"
            };

            var response = await _client.PostAsJsonAsync("/api/parcels", registerParcelRequest);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetParcelResponse>();

            Assert.NotNull(result);
            Assert.Equal(registerParcelRequest.Barcode, result.Barcode);
        }
    }
}
