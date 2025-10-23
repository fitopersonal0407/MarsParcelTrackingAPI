using MarsParcelTracking.Domain;

namespace MarsParcelTracking.Application
{
    public class ParcelService : IParcelService
    {
        private readonly IParcelDataAccess _dataAccess;
        internal const string PARCELORIGIN = "Starport Thames Estuary";
        internal const string PARCELRECIPIENT = "New London";

        public ParcelService(IParcelDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<List<ParcelDTO>> GetGetParcelsAsync()
        {
            var parcels = await _dataAccess.GetAllParcelsAsync();
            return parcels.Select(p => ItemToDTO(p)).ToList();
        }

        public async Task<ParcelDTO> GetParcelAsync(long id)
        {
            var parcel = await _dataAccess.FindAsync(id);

            if (parcel == null)
                return null;
            else
                return ItemToDTO(parcel);
        }

        public async Task<ServiceResponse1<ParcelDTO>> RegisterParcelAsync(ParcelDTO parcelDTO)
        {
            var answer = new ServiceResponse1<ParcelDTO>();

            if (!ValidateBarCode(parcelDTO.Barcode))
            {
                answer.Response = ServiceResponseCode.BarcodeInvalid;
                return answer;
            }
            else
                try
                {
                    var deliveryService = (DeliveryService)Enum.Parse(typeof(DeliveryService), parcelDTO.DeliveryService);
                    var launchDate = ComputeLaunchDate(deliveryService);
                    var etaDays = ComputeEtaDays();
                    var estimatedArrivalDate = ComputeEstimatedArrivalDate(launchDate, etaDays);

                    var parcel = new Parcel
                    {
                        Barcode = parcelDTO.Barcode,
                        Status = ParcelStatus.Created,
                        Sender = PARCELORIGIN,
                        Recipient = PARCELRECIPIENT,
                        DeliveryService = deliveryService,
                        Contents = parcelDTO.Contents,
                        LaunchDate = launchDate,
                        EtaDays = etaDays,
                        EstimatedArrivalDate = estimatedArrivalDate,
                    };

                    answer.Data = ItemToDTO(await _dataAccess.Add(parcel));
                    return answer;
                }
                catch (Exception ex)
                {
                    answer.Response = ServiceResponseCode.UnexpectedError;
                    answer.Message = ex.Message;
                    return answer;
                }
        }

        private static ParcelDTO ItemToDTO(Parcel i) =>
                    new ParcelDTO
                    {
                        Barcode = i.Barcode,
                        Status = nameof(i.Status),
                        Sender = i.Sender,
                        Recipient = i.Recipient,
                        DeliveryService = nameof(i.DeliveryService),
                        Contents = i.Contents,
                        LaunchDate = Util.UTCDateToString(i.LaunchDate),
                        EtaDays = i.EtaDays,
                        EstimatedArrivalDate = Util.UTCDateToString(i.EstimatedArrivalDate)
                    };

        private bool ValidateBarCode(string? barcode)
        {
            var answer = false;
            if (!string.IsNullOrWhiteSpace(barcode))
            {
                //todo this is invalid
                answer = true;
            }
            return answer;
        }

        private DateTime ComputeLaunchDate(DeliveryService deliveryService)
        {
            var answer = DateTime.UtcNow;
            //TODO: this is wrong
            return answer;
        }

        private int ComputeEtaDays()
        {
            var answer = 1;
            //TODO: this is wrong
            return answer;
        }

        private DateTime ComputeEstimatedArrivalDate(DateTime launchDate, int etaDays)
        {
            var answer = DateTime.UtcNow;
            //TODO: this is wrong
            return answer;
        }
    }
}
