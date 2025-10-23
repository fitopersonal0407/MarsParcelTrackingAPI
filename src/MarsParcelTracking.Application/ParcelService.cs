using MarsParcelTracking.Domain;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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

        public async Task<ServiceResponse<ParcelDTO>> RegisterParcelAsync(ParcelDTO parcelDTO)
        {
            var answer = new ServiceResponse<ParcelDTO>();

            if (!ValidateBarCode(parcelDTO.Barcode))
            {
                answer.Response = ServiceResponseCode.BarcodeInvalid;
                answer.Message = "BarcodeInvalid";
                return answer;
            }
            else
                try
                {
                    var deliveryService = (DeliveryService)Enum.Parse(typeof(DeliveryService), parcelDTO.DeliveryService);
                    var launchDate = ComputeLaunchDate(deliveryService);
                    var etaDays = ComputeEtaDays(deliveryService);
                    var estimatedArrivalDate = ComputeEstimatedArrivalDate(launchDate, etaDays);

                    var parcel = new Parcel
                    {
                        Barcode = parcelDTO.Barcode,
                        Status = ParcelStatus.Created,
                        Sender = parcelDTO.Sender,
                        Recipient = parcelDTO.Recipient,
                        Origin = PARCELORIGIN,
                        Destination = PARCELRECIPIENT,
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
                        Origin = i.Origin,
                        Destination = i.Destination,
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
                var pattern = @"^RMARS\d{19}[A-Z]$";
                answer = Regex.IsMatch(input: barcode, pattern: pattern);
            }
            return answer;
        }

        private DateTime ComputeLaunchDate(DeliveryService deliveryService)
        {
            var now = DateTime.UtcNow;
            var nextStandardLaunchDate = Util.StringToUTCDate("2025-10-01T00:00:00.000Z").Value;
            switch (deliveryService)
            {
                case DeliveryService.Standard:
                    return now <= nextStandardLaunchDate ? nextStandardLaunchDate : nextStandardLaunchDate.AddMonths(26);
                case DeliveryService.Express:
                    var year = now.Year;
                    var month = now.Month;

                    var firstWednesday = GetFirstWednesday(year, month);
                    if (now <= firstWednesday)
                        return firstWednesday;
                    else
                    {
                        if (month == 12)
                        {
                            year++;
                            month = 1;
                        }
                        else
                            month++;

                        return GetFirstWednesday(year, month);
                    }
                default: throw new ArgumentException("deliveryService");
            }
        }

        public static DateTime GetFirstWednesday(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);
            var daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)firstDay.DayOfWeek + 7) % 7;
            return firstDay.AddDays(daysUntilWednesday);
        }

        private int ComputeEtaDays(DeliveryService deliveryService)
        {
            switch (deliveryService)
            {
                case DeliveryService.Standard:
                    return 180;
                case DeliveryService.Express:
                    return 90;
                default: throw new ArgumentException("deliveryService");
            }
        }

        private DateTime ComputeEstimatedArrivalDate(DateTime launchDate, int etaDays)
        {
            return launchDate.AddDays(etaDays); ;
        }
    }
}
