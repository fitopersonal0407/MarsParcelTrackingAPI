using MarsParcelTracking.API.Responses;
using Microsoft.AspNetCore.Components;

namespace MarsParcelTracker.Blazor.Components.Pages
{
    public partial class GetParcels : ComponentBase
    {
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
                var response = await _httpClient.GetAsync($"/api/parcels/");
                entities = await response.Content.ReadFromJsonAsync<List<GetParcelResponse>>();
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
