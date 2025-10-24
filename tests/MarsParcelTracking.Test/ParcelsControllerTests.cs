using MarsParcelTracking.API;
using MarsParcelTracking.API.Controllers;
using MarsParcelTracking.API.Responses;
using MarsParcelTracking.Application;
using MarsParcelTracking.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace MarsParcelTracking.Test
{
    [TestClass]
    public sealed class ParcelsControllerTests
    {
        private IParcelDataAccess _dataAccess;
        private IParcelService _service;
        private ParcelsController _controller;

        [TestInitialize]
        public void Initialize()
        {
            _dataAccess = GetDataAccessReal();
            _service = new ParcelService(_dataAccess);
            _controller = new ParcelsController(_service);
        }

        private IParcelDataAccess GetDataAccessReal()
        {
            var options = new DbContextOptionsBuilder<ParcelContext>()
                .UseInMemoryDatabase(databaseName: $"TestDatabase_{Guid.NewGuid()}")
                .Options;
            ParcelContext _context = new ParcelContext(options);
            _context.Database.EnsureCreated();
            var answer = new ParcelDataAccessEF(_context);
            return answer;
        }

        [TestMethod]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            var barcode = "RMARS1234567890123456789A";

            var request = new RegisterParcelRequest()
            {
                Barcode = barcode,
                Sender = "Cuco Malanga",
                Recipient = "Conde Monte Cristo",
                DeliveryService = "Standard",
                Contents = "Express",
            };

            {
                var result = await _controller.RegisterParcel(request);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
                var okResult = result.Result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.IsNotNull(okResult.Value);
                GetParcelResponse response = okResult.Value as GetParcelResponse;

                Assert.AreEqual(expected: request.Barcode, actual: response.Barcode);
                Assert.AreEqual(expected: EnumParcelStatus.Created.ToString(), actual: response.Status);
                Assert.AreEqual(expected: request.Sender, actual: response.Sender);
                Assert.AreEqual(expected: request.Recipient, actual: response.Recipient);
                Assert.AreEqual(expected: IParcelService.PARCELORIGIN, actual: response.Origin);
                Assert.AreEqual(expected: IParcelService.PARCELDESTINATION, actual: response.Destination);
                Assert.AreEqual(expected: request.DeliveryService, actual: response.DeliveryService);
                Assert.AreEqual(expected: request.Contents, actual: response.Contents);
            }

            {
                var result = await _controller.GetParcel(barcode);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
                var okResult = result.Result as OkObjectResult;
                Assert.IsNotNull(okResult);
                Assert.IsNotNull(okResult.Value);
                GetParcelWithHistoryResponse response = okResult.Value as GetParcelWithHistoryResponse;
                Assert.AreEqual(expected: request.Barcode, actual: response.Barcode);
            }
        }
    }
}
