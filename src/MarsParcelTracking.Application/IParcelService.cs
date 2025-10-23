namespace MarsParcelTracking.Application
{
    public interface IParcelService
    {
        Task<List<ParcelDTO>> GetGetParcelsAsync();
        Task<ParcelDTO> GetParcelAsync(long id);
        Task<ServiceResponse<ParcelDTO>> RegisterParcelAsync(ParcelDTO parcelDTO);
        Task<ServiceResponse<ParcelDTO>> ChangeParcelStatusAsync(ParcelDTO parcelDTO);
    }
}
