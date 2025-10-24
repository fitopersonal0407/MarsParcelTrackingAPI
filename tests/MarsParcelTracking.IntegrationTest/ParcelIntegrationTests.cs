using MarsParcelTracking.API;
using MarsParcelTracking.API.Responses;
using MarsParcelTracking.Application;
using System.Net.Http.Json;

namespace MarsParcelTracking.IntegrationTest
{
    public class ParcelIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        private const string BARCODE_001 = "RMARS1234567890123456789A";

        public ParcelIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterParcel_ValidRequest_ReturnsSuccess()
        {
            var request = new RegisterParcelRequest()
            {
                Barcode = BARCODE_001,
                Sender = "Cuco Malanga",
                Recipient = "Conde Monte Cristo",
                DeliveryService = "Standard",
                Contents = "Express",
            };

            var response = await _client.PostAsJsonAsync("/api/parcels", request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetParcelResponse>();
            Assert.NotNull(result);
            Assert.Equal(expected:request.Barcode,actual: result.Barcode);
            Assert.Equal(expected: EnumParcelStatus.Created.ToString(), actual: result.Status);
            Assert.Equal(expected: request.Sender, actual: result.Sender);
            Assert.Equal(expected: request.Recipient, actual: result.Recipient);
            Assert.Equal(expected: IParcelService.PARCELORIGIN, actual: result.Origin);
            Assert.Equal(expected: IParcelService.PARCELDESTINATION, actual: result.Destination);
            Assert.Equal(expected: request.DeliveryService, actual: result.DeliveryService);
            Assert.Equal(expected: request.Contents, actual: result.Contents);
        }
    }
}
