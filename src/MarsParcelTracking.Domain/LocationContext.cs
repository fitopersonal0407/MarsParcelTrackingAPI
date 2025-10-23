using Microsoft.EntityFrameworkCore;

namespace MarsParcelTracking.Domain
{
    public class LocationContext : DbContext
    {
        public LocationContext(DbContextOptions<LocationContext> options)
        : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; } = null!;
    }
}
