using MarsParcelTracking.Domain;
using Microsoft.EntityFrameworkCore;

namespace MarsParcelTracking.Application
{
    public class ParcelDataAccessEF : IParcelDataAccess
    {
        private readonly ParcelContext _context;

        public ParcelDataAccessEF(ParcelContext context)
        {
            _context = context;
        }

        public async Task<List<Parcel>> GetAllParcelsAsync()
        {
            return await _context.Parcels.ToListAsync();
        }

        public async Task<Parcel> FindAsync(long id)
        {
            return await _context.Parcels.FindAsync(id);
        }

        public async Task<Parcel> FindAsync(string barcode)
        {
            return await _context.Parcels.FirstOrDefaultAsync(p => p.Barcode == barcode);
        }

        public async Task<Parcel> Add(Parcel parcel)
        {
            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync();
            return await FindAsync(parcel.Id);
        }

        public async Task<Parcel> Update(Parcel parcel)
        {
            _context.Parcels.Update(parcel);
            await _context.SaveChangesAsync();
            return await FindAsync(parcel.Id);
        }
    }
}
