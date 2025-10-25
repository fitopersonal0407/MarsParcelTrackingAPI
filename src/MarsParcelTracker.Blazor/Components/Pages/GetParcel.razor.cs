namespace MarsParcelTracker.Blazor.Components.Pages
{
    public partial class GetParcel : ParcelsBase
    {
        private int currentCount = 0;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void IncrementCount()
        {
            currentCount++;
        }
    }
}
