#pragma warning disable CS8601
#pragma warning disable CS8602
#pragma warning disable CS8618

using Microsoft.AspNetCore.Components;
using MarsParcelTracker.Blazor.Components.Models;

namespace MarsParcelTracker.Blazor.Components.Pages
{
    public partial class GetParcel : ParcelsBase
    {
        private string _barcode = string.Empty;
        private GetParcelWithHistoryResponse _entity;
        private bool _entityHasBeenSearched = false;

        protected bool isLoading = false;
        protected bool showHelp = false;
        protected string validationMessage = string.Empty;
        protected string resultMessage = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _barcode = "RMARS1234567890123456789A";
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
                _entityHasBeenSearched = true;
                var response = await _httpClient.GetAsync($"/api/parcels/{_barcode}");
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    _entity = await response.Content.ReadFromJsonAsync<GetParcelWithHistoryResponse>();
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

#pragma warning restore CS8601
#pragma warning restore CS8602
#pragma warning restore CS8618