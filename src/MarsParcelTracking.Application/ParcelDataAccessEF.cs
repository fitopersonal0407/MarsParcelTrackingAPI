#pragma warning disable CS8603

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
            var answer = await _context.Parcels.Include(p => p.History).ToListAsync();
            return answer;
        }

        public async Task<Parcel> FindAsync(long id)
        {
            //return await _context.Parcels.FindAsync(id);
            var answer = await _context.Parcels.Include(p => p.History).FirstOrDefaultAsync(p => p.Id == id);
            return answer;
        }

        public async Task<Parcel> FindAsync(string barcode)
        {
            var answer = await _context.Parcels.Include(p => p.History).FirstOrDefaultAsync(p => p.Barcode == barcode);
            return answer;
        }

        public async Task<Parcel> Add(Parcel parcel)
        {
            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync();
            var answer = await FindAsync(parcel.Id);
            return answer;
        }

        public async Task<Parcel> Update(Parcel parcel)
        {
            _context.Parcels.Update(parcel);
            await _context.SaveChangesAsync();
            return await FindAsync(parcel.Id);
        }
    }
}

#pragma warning restore CS8603