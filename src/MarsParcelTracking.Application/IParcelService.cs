namespace MarsParcelTracking.Application
{
    public interface IParcelService
    {
        Task<List<ParcelDTO>> GetParcelsAsync();
        Task<ParcelDTO> GetParcelAsync(long id);
        Task<ParcelDTO> GetParcelAsync(string barcode);
        Task<ServiceResponse<ParcelDTO>> RegisterParcelAsync(ParcelDTO parcelDTO);
        Task<ServiceResponse<ParcelDTO>> ChangeParcelStatusAsync(ParcelDTO parcelDTO);
    }
}
