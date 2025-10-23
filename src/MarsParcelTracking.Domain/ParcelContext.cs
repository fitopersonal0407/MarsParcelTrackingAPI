using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarsParcelTracking.Domain
{
    public class ParcelContext : DbContext
    {
        public ParcelContext(DbContextOptions<ParcelContext> options)
        : base(options)
        {
        }

        public DbSet<Parcel> Parcels { get; set; } = null!;
        public DbSet<ParcelTransition> ParcelTransitions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parcel>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.HasMany(p => p.History)
                      .WithOne()
                      .HasForeignKey(pt => pt.ParcelId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ParcelTransition>(entity =>
            {
                entity.HasKey(pt => pt.Id);
            });
        }
    }
}