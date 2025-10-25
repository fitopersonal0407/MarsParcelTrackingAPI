#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8629

using MarsParcelTracking.API;
using MarsParcelTracking.API.Responses;
using MarsParcelTracking.Application;
using System.Net;
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
        public async Task RegisterAndFetch()
        {
            var postRequest = new RegisterParcelRequest()
            {
                Barcode = BARCODE_001,
                Sender = "Cuco Malanga",
                Recipient = "Conde Monte Cristo",
                DeliveryService = "Standard",
                Contents = "Express",
            };

            var correctPatchRequest = new PatchParcelRequest()
            {
                Status = EnumParcelStatus.OnRocketToMars.ToString()
            };

            #region register and fetching

            {
                var response = await _client.PostAsJsonAsync("/api/parcels", postRequest);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<GetParcelResponse>();
                Compare(postRequest, result);
            }

            {
                var response = await _client.GetAsync($"/api/parcels/{postRequest.Barcode}");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<GetParcelWithHistoryResponse>();
                Compare(postRequest, result);

                Assert.NotNull(result.History);
                Assert.Single(result.History);
                Assert.Collection(result.History,
                                                    item1 => Assert.Equal(EnumParcelStatus.Created.ToString(), item1.Status)
                                    );
            }

            #endregion

            #region status change, correct

            {
                var response = await _client.PatchAsJsonAsync($"/api/parcels/{postRequest.Barcode}", correctPatchRequest);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            {
                var response = await _client.GetAsync($"/api/parcels/{postRequest.Barcode}");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<GetParcelWithHistoryResponse>();
                Assert.NotNull(result.History);
                Assert.Equal(expected: 2, actual: result.History.Count());
                Assert.Collection(result.History,
                                                    item1 => Assert.Equal(EnumParcelStatus.Created.ToString(), item1.Status),
                                                    item2 => Assert.Equal(correctPatchRequest.Status, item2.Status)
                                    );
            }

            #endregion

            #region status change, wrong

            {
                var wrongPatchRequest = new PatchParcelRequest()
                {
                    Status = EnumParcelStatus.Created.ToString()
                };

                var response = await _client.PatchAsJsonAsync($"/api/parcels/{postRequest.Barcode}", wrongPatchRequest);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            }

            {
                var response = await _client.GetAsync($"/api/parcels/{postRequest.Barcode}");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<GetParcelWithHistoryResponse>();
                Assert.NotNull(result.History);
                Assert.Equal(expected: 2, actual: result.History.Count());
                Assert.Collection(result.History,
                                                    item1 => Assert.Equal(EnumParcelStatus.Created.ToString(), item1.Status),
                                                    item2 => Assert.Equal(correctPatchRequest.Status, item2.Status)
                                    );
            }

            #endregion
        }

        private void Compare(RegisterParcelRequest request, GetParcelResponse result)
        {
            Assert.NotNull(result);
            Assert.Equal(expected: request.Barcode, actual: result.Barcode);
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

#pragma warning restore CS8601
#pragma warning restore CS8602
#pragma warning restore CS8604
#pragma warning restore CS8629