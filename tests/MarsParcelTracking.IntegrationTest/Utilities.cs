using MarsParcelTracking.Domain;

namespace MarsParcelTracking.IntegrationTest
{
    public static class Utilities
    {
        public static void InitializeDbForTests(ParcelContext db)
        {
            // Remove all existing data from ParcelTransitions table for a clean slate
            db.ParcelTransitions.RemoveRange(db.ParcelTransitions);

            // Remove all existing data from Parcels table to avoid duplicate key errors and stale data
            db.Parcels.RemoveRange(db.Parcels);

            // Persist all deletions to the in-memory database
            db.SaveChanges();

            {
                var transition = new ParcelTransition { Id = 1, Status = EnumParcelStatus.Created, Timestamp = DateTime.UtcNow };
                var parcel = new Parcel { Id = 1, Barcode = "asasasas" ,History= new List<ParcelTransition> { transition} };
                transition.ParcelId = parcel.Id;

                db.ParcelTransitions.AddRange(transition);
                db.Parcels.AddRange(parcel);
            }
            db.SaveChanges();
        }
    }
}
