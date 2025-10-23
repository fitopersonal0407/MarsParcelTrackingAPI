using MarsParcelTracking.Domain;

namespace MarsParcelTracking.Application
{
    public interface IParcelService
    {
        Task<List<ParcelDTO>> GetGetParcelsAsync();
        Task<ServiceResponse1<ParcelDTO>> RegisterParcelAsync(ParcelDTO parcelDTO);
    }
}
