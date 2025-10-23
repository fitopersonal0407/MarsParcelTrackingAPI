using MarsParcelTracking.Domain;
using Microsoft.EntityFrameworkCore;

namespace MarsParcelTracking.Application
{
    public class LocationService
    {
        private readonly LocationContext _context;

        public LocationService(LocationContext context)
        {
            _context = context;
        }

        #region Locations

        public async Task<List<LocationDTO>> GetLocationsAsync()
        {
            return await _context.Locations.Select(x => ItemToDTO(x)).ToListAsync();
        }

        public async Task<LocationDTO> GetLocationAsync(long id)
        {
            var location = await _context.Locations.FindAsync(id);

            if (location == null)
                return null;
            else
                return ItemToDTO(location);
        }

        public async Task<ServiceResponse<LocationDTO>> UpdateLocation(long id, LocationDTO locationDTO)
        {
            var answer = new ServiceResponse<LocationDTO>();

            if (id != locationDTO.Id)
            {
                answer.Success = false;
                answer.Message = "BadRequest, Ids are not the same";
                return answer;
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                answer.Success = false;
                answer.Message = "NotFound";
                return answer;
            }

            location.Planet = locationDTO.Planet;
            answer.Data = ItemToDTO(location);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!LocationExists(id))
            {
                if (!LocationExists(id))
                {
                    answer.Success = false;
                    answer.Message = "NotFound";
                    return answer;
                }
                else
                {
                    throw;
                }
            }

            return answer;
        }

        public async Task<ServiceResponse<LocationDTO>> PostLocation(LocationDTO locationDTO)
        {
            var location = new Location
            {
                Planet = locationDTO.Planet,
                City = null
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            var answer = new ServiceResponse<LocationDTO>() { Data = await GetLocationAsync(location.Id) };
            return answer;
        }

        public async Task<ServiceResponse<LocationDTO>> DeleteLocation(long id)
        {
            var answer = new ServiceResponse<LocationDTO>();

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                answer.Success = false;
                answer.Message = "NotFound";
                return answer;
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return answer;
        }

        private bool LocationExists(long id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }

        private static LocationDTO ItemToDTO(Location i) =>
            new LocationDTO
            {
                Id = i.Id,
                Planet = i.Planet
            };

        #endregion
    }
}
