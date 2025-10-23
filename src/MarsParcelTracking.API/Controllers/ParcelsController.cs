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
