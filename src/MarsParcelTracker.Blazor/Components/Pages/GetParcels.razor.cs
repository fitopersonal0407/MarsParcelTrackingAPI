#pragma warning disable CS8602

using MarsParcelTracker.Blazor.Models;

namespace MarsParcelTracker.Blazor.Components.Pages
{
    public partial class GetParcels : ParcelsBase
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadData();
        }


        protected List<GetParcelResponse>? entities;
        protected bool isLoading = false;
        protected bool hasError = false;
        protected string errorMessage = string.Empty;

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

#pragma warning restore CS8602