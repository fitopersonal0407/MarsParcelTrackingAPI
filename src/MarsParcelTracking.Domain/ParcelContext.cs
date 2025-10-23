using Microsoft.EntityFrameworkCore;

namespace MarsParcelTracking.Domain
{
    public class ParcelContext : DbContext
    {
        public ParcelContext(DbContextOptions<ParcelContext> options)
        : base(options)
        {
        }

        public DbSet<Parcel> Parcels { get; set; } = null!;
    }
}