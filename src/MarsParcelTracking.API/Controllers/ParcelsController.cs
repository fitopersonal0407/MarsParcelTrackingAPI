using MarsParcelTracking.API.Responses;
using MarsParcelTracking.Application;
using Microsoft.AspNetCore.Mvc;

namespace MarsParcelTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelsController : ControllerBase
    {
        private readonly IParcelService _service;

        public ParcelsController(IParcelService service)
        {
            _service = service;
        }

        // GET: api/Parcels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetParcelResponse>>> GetParcels()
        {
            var result = await _service.GetGetParcelsAsync();
            return result.Select(i => DTOToResponse(i)).ToList();
        }

        // GET: api/Parcels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetParcelResponse>> GetParcel(long id)
        {
            var result = await _service.GetParcelAsync(id);
            if (result == null)
                return NoContent();
            else
                return DTOToResponse(result);
        }

        // POST: api/Parcels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetParcelResponse>> RegisterParcel(RegisterParcelRequest registerParcelRequest)
        {
            var parcelDTO = new ParcelDTO
            {
                Barcode = registerParcelRequest.Barcode,
                Sender = registerParcelRequest.Sender,
                Recipient = registerParcelRequest.Recipient,
                DeliveryService = registerParcelRequest.DeliveryService,
                Contents = registerParcelRequest.Contents,
            };

            var result = await _service.RegisterParcelAsync(parcelDTO);
            if (result.Success)
                return Ok(DTOToResponse(result.Data));
            else
                switch (result.Response)
                {
                    case ServiceResponseCode.BarcodeInvalid: return BadRequest(result.Message);
                    default: return StatusCode(500, result.Message);
                }
        }

        //PATCH: api/Parcels
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{barcode}")]
        public async Task<ActionResult<GetParcelResponse>> PatchParcel(PatchParcelRequest patchParcelRequest, string barcode)
        {
            var parcelDTO = new ParcelDTO
            {
                Barcode = barcode,
                Status = patchParcelRequest.Status,
            };

            var result = await _service.ChangeParcelStatusAsync(parcelDTO);
            if (result.Success)
                return Ok(DTOToResponse(result.Data));
            else
                switch (result.Response)
                {
                    case ServiceResponseCode.BarcodeInvalid:
                    case ServiceResponseCode.BarcodeNotExist:
                    case ServiceResponseCode.StatusInvalid:
                    case ServiceResponseCode.StatusTransitionInvalid: return BadRequest(result.Message);
                    default: return StatusCode(500, result.Message);
                }
        }

        private static GetParcelResponse DTOToResponse(ParcelDTO i) =>
                    new GetParcelResponse
                    {
                        Barcode = i.Barcode,
                        Status = i.Status,
                        Sender = i.Sender,
                        Recipient = i.Recipient,
                        Origin = i.Origin,
                        Destination = i.Destination,
                        DeliveryService = i.DeliveryService,
                        Contents = i.Contents,
                        LaunchDate = i.LaunchDate,
                        EtaDays = i.EtaDays,
                        EstimatedArrivalDate = i.EstimatedArrivalDate,
                    };
    }
}
