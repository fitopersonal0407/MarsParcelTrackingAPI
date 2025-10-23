using MarsParcelTracking.Domain;

namespace MarsParcelTracking.Application
{
    public interface IParcelDataAccess
    {
        Task<List<Parcel>> GetAllParcelsAsync();
        Task<Parcel> FindAsync(long id);
        Task<Parcel> Add(Parcel parcel);
    }
}
