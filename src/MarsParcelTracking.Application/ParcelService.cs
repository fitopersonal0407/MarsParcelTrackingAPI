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

        public async Task<List<ParcelDTO>> GetParcelsAsync()
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

        public async Task<ParcelDTO> GetParcelAsync(string barcode)
        {
            var parcel = await _dataAccess.FindAsync(barcode);

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

            var existing = await _dataAccess.FindAsync(parcelDTO.Barcode);
            if (existing != null)
            {
                answer.Response = ServiceResponseCode.DuplicatedBarcode;
                answer.Message = "DuplicatedBarcode";
                return answer;
            }

            try
            {
                var deliveryService = (EnumDeliveryService)Enum.Parse(typeof(EnumDeliveryService), parcelDTO.DeliveryService);
                var launchDate = ComputeLaunchDate(deliveryService);
                var etaDays = ComputeEtaDays(deliveryService);
                var estimatedArrivalDate = ComputeEstimatedArrivalDate(launchDate, etaDays);

                var parcel = new Parcel
                {
                    Barcode = parcelDTO.Barcode,
                    Status = EnumParcelStatus.Created,
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

                var result = await _dataAccess.Add(parcel);
                answer.Data = ItemToDTO(result);
                return answer;
            }
            catch (Exception ex)
            {
                answer.Response = ServiceResponseCode.UnexpectedError;
                answer.Message = ex.Message;
                return answer;
            }
        }

        public async Task<ServiceResponse<ParcelDTO>> ChangeParcelStatusAsync(ParcelDTO parcelDTO)
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
                    var parcel = await _dataAccess.FindAsync(parcelDTO.Barcode);
                    if (parcel == null)
                    {
                        answer.Response = ServiceResponseCode.BarcodeNotExist;
                        answer.Message = "BarcodeNotExist";
                        return answer;
                    }

                    var newStatus = EnumParcelStatus.Created;
                    try
                    {
                        newStatus = (EnumParcelStatus)Enum.Parse(typeof(EnumParcelStatus), parcelDTO.Status);
                        if (!ValidateStatusTransitions(currentStatus: parcel.Status, newStatus))
                        {
                            answer.Response = ServiceResponseCode.StatusTransitionInvalid;
                            answer.Message = "StatusTransitionInvalid";
                            return answer;
                        }
                    }
                    catch
                    {
                        answer.Response = ServiceResponseCode.StatusInvalid;
                        answer.Message = "StatusInvalid";
                        return answer;
                    }

                    parcel.Status = newStatus;
                    answer.Data = ItemToDTO(await _dataAccess.Update(parcel));
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
                        Status = i.Status.ToString(),
                        Sender = i.Sender,
                        Recipient = i.Recipient,
                        Origin = i.Origin,
                        Destination = i.Destination,
                        DeliveryService = i.DeliveryService.ToString(),
                        Contents = i.Contents,
                        LaunchDate = Util.UTCDateToString(i.LaunchDate),
                        EtaDays = i.EtaDays,
                        EstimatedArrivalDate = Util.UTCDateToString(i.EstimatedArrivalDate),
                        History = i.History,
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

        private bool ValidateStatusTransitions(EnumParcelStatus currentStatus, EnumParcelStatus newStatus)
        {
            var answer = false;
            var allowedTransitions = new List<EnumParcelStatus>();
            switch (currentStatus)
            {
                case EnumParcelStatus.Created:
                    allowedTransitions.Add(EnumParcelStatus.OnRocketToMars);
                    break;
                case EnumParcelStatus.OnRocketToMars:
                    allowedTransitions.Add(EnumParcelStatus.LandedOnMars);
                    allowedTransitions.Add(EnumParcelStatus.Lost);
                    break;
                case EnumParcelStatus.LandedOnMars:
                    allowedTransitions.Add(EnumParcelStatus.OutForMartianDelivery);
                    break;
                case EnumParcelStatus.OutForMartianDelivery:
                    allowedTransitions.Add(EnumParcelStatus.Delivered);
                    allowedTransitions.Add(EnumParcelStatus.Lost);
                    break;
                case EnumParcelStatus.Delivered:
                    allowedTransitions.Add(EnumParcelStatus.Delivered);
                    break;
                case EnumParcelStatus.Lost:
                    allowedTransitions.Add(EnumParcelStatus.Lost);
                    break;
            }
            answer = allowedTransitions.Contains(newStatus);
            return answer;
        }

        private DateTime ComputeLaunchDate(EnumDeliveryService deliveryService)
        {
            var now = DateTime.UtcNow;
            var nextStandardLaunchDate = Util.StringToUTCDate("2025-10-01T00:00:00.000Z").Value;
            switch (deliveryService)
            {
                case EnumDeliveryService.Standard:
                    return now <= nextStandardLaunchDate ? nextStandardLaunchDate : nextStandardLaunchDate.AddMonths(26);
                case EnumDeliveryService.Express:
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

        private static DateTime GetFirstWednesday(int year, int month)
        {
            var firstDay = new DateTime(year, month, 1);
            var daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)firstDay.DayOfWeek + 7) % 7;
            return firstDay.AddDays(daysUntilWednesday);
        }

        private int ComputeEtaDays(EnumDeliveryService deliveryService)
        {
            switch (deliveryService)
            {
                case EnumDeliveryService.Standard:
                    return 180;
                case EnumDeliveryService.Express:
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
