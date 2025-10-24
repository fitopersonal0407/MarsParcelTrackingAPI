using MarsParcelTracking.API.Responses;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Json;

namespace MarsParcelTracker.Blazor.Components.Pages
{


    public partial class EntitiesPageBase : ComponentBase
    {
        //[Inject]
        //protected HttpClient _httpClient { get; set; } = null!;

        [Inject]
        protected IHttpClientFactory HttpClientFactory { get; set; } = null!;
        protected HttpClient? _httpClient;

        protected List<GetParcelResponse>? entities;
        protected bool isLoading = false;
        protected bool hasError = false;
        protected string errorMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            _httpClient = HttpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7168/");
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

            await LoadData();
        }

        protected async Task LoadData()
        {
            isLoading = true;
            hasError = false;
            errorMessage = string.Empty;
            StateHasChanged();

            try
            {
                var barcode = "RMARS1234567890123456789A";
                var response = await _httpClient.GetAsync($"/api/parcels/{barcode}");
                var result = await response.Content.ReadFromJsonAsync<GetParcelWithHistoryResponse>();
                entities = new List<GetParcelResponse> { result };
            }
            catch (Exception ex)
            {
                hasError = true;
                errorMessage = ex.Message;
                entities = new List<GetParcelResponse>();
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }
    }
}
