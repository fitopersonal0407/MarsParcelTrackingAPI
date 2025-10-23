using Microsoft.AspNetCore.Mvc;
using MarsParcelTracking.Domain;
using MarsParcelTracking.Application;

namespace MarsParcelTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly LocationService _service;

        public LocationsController(LocationService service)
        {
            _service = service;
        }

        // GET: api/Locations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDTO>>> GetLocations()
        {
            return await _service.GetLocationsAsync();
        }

        // GET: api/Locations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDTO>> GetLocation(long id)
        {
            var location = await _service.GetLocationAsync(id);
            if (location == null)
                return NoContent();
            else
                return location;
        }

        // PUT: api/Locations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(long id, LocationDTO locationDTO)
        {
            var answer = await _service.UpdateLocation(id, locationDTO);

            if (!answer.Success)
            {
                switch (answer.Message)
                {
                    case "NotFound": return NotFound();
                    case "BadRequest, Ids are not the same": return BadRequest(answer.Message);
                    default: return StatusCode(500, answer.Message);
                }
            }
            else
                return NoContent();
        }

        // POST: api/Locations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LocationDTO>> PostLocation(LocationDTO locationDTO)
        {
            var answer = await _service.PostLocation(locationDTO);
            if (!answer.Success)
                return StatusCode(500, answer.Message);
            else
                return Ok(answer.Data);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(long id)
        {
            var answer = await _service.DeleteLocation(id);
            if (!answer.Success)
            {
                switch (answer.Message)
                {
                    case "NotFound": return NotFound();
                    default: return StatusCode(500, answer.Message);
                }
            }
            else
                return NoContent();
        }
    }
}
