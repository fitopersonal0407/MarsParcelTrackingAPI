using MarsParcelTracking.API.Responses;
using Microsoft.AspNetCore.Components;

namespace MarsParcelTracker.Blazor.Components.Pages
{
    public partial class GetParcel : ParcelsBase
    {
        private string _barcode = string.Empty;

        protected bool isLoading = false;
        protected bool showHelp = false;
        protected string validationMessage = string.Empty;
        protected string resultMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(_barcode))
            {
                validationMessage = "Please enter a value for the barcode";
                return false;
            }
            validationMessage = string.Empty;
            return true;
        }

        protected async Task Submit()
        {
            if (!ValidateInput())
            {
                return;
            }

            isLoading = true;

            try
            {
                var response = await _httpClient.GetAsync($"/api/parcels/{_barcode}");
                var entity = await response.Content.ReadFromJsonAsync<GetParcelWithHistoryResponse>();

                resultMessage = $"Successfully processed the barcode: '{entity.Contents}'";
            }
            catch (Exception ex)
            {
                resultMessage = $"Error processing the barcode: {ex.Message}";
            }
            finally
            {
                isLoading = false;
            }
        }

        protected void Clear()
        {
            _barcode = string.Empty;
            validationMessage = string.Empty;
            resultMessage = string.Empty;
            StateHasChanged();
        }

        protected void ShowHelp()
        {
            showHelp = true;
        }

        protected void CloseHelp()
        {
            showHelp = false;
        }

        protected void OnInputChanged(ChangeEventArgs e)
        {
            _barcode = e.Value?.ToString() ?? string.Empty;
            ValidateInput();
        }
    }
}
