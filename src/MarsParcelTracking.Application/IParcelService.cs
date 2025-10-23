using MarsParcelTracking.Domain;

namespace MarsParcelTracking.Application
{
    public interface IParcelService
    {
        Task<List<ParcelDTO>> GetGetParcelsAsync();
        Task<ServiceResponse<ParcelDTO>> RegisterParcelAsync(ParcelDTO parcelDTO);
    }
}
