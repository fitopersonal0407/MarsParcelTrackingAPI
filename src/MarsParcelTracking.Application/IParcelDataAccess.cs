using MarsParcelTracking.Domain;

namespace MarsParcelTracking.Application
{
    public interface IParcelDataAccess
    {
        Task<List<Parcel>> GetAllParcelsAsync();
        Task<Parcel> FindAsync(long id);
        Task<Parcel> FindAsync(string barcode);
        Task<Parcel> Add(Parcel parcel);
    }
}
