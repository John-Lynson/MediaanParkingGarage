using CORE.Entities;
using CORE.Interfaces.IRepositories;
using CORE.Services;
using Mollie.Api.Client;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

namespace ParkingGarage.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _mockPaymentRepo;
        private Mock<ISpotOccupationRepository> _mockSpotOccupationRepo;
        private Mock<IAccountRepository> _mockAccountRepo;
        private Mock<ICarRepository> _mockCarRepo;
        private PaymentService _paymentService;
        private SpotOccupation _testSpotOccupation;
        private DateTime _entryTime;
        private DateTime _exitTime;
        private IConfiguration _configuration;
        private PaymentClient _molliePaymentClient;

        [TestInitialize]
        public void SetUp()
        {
            // Mock setup
            _mockPaymentRepo = new Mock<IPaymentRepository>();
            _mockSpotOccupationRepo = new Mock<ISpotOccupationRepository>();
            _mockAccountRepo = new Mock<IAccountRepository>();
            _mockCarRepo = new Mock<ICarRepository>();

            // Mock configuration
            var inMemorySettings = new Dictionary<string, string> {
                {"Mollie:ApiKey", "fake_api_key"},
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Initialize a real MolliePaymentClient with the fake API key
            _molliePaymentClient = new PaymentClient(_configuration["Mollie:ApiKey"]);

            // Set up a test SpotOccupation record
            _entryTime = DateTime.Now.AddHours(-3); // 3 hours ago
            _exitTime = DateTime.Now;
            _testSpotOccupation = new SpotOccupation
            {
                CarId = 1,
                ParkingSpotId = 1,
                ActualStartDate = _entryTime,
                ActualEndDate = _exitTime
            };

            // Set up the mock to return the test SpotOccupation record
            _mockSpotOccupationRepo.Setup(repo => repo.GetLatestByCarId(It.IsAny<int>())).Returns(_testSpotOccupation);

            // Initialize the service with the mocked repositories and real Mollie client
            _paymentService = new PaymentService(_mockPaymentRepo.Object, _mockSpotOccupationRepo.Object, _mockAccountRepo.Object, _mockCarRepo.Object, _configuration);
        }

        [TestMethod]
        public async Task ProcessPaymentAsync_CalculatesCorrectFee()
        {
            // Arrange
            int ratePerHour = 300;
            int expectedFee = (int)((_exitTime - _entryTime).TotalHours * ratePerHour); // Expected fee in cents
            int carId = 1;
            int garageId = 1;
            DateTime paymentDate = DateTime.Now;
            string fakeRedirectUrl = "url";

            var fakeMolliePaymentResponse = new PaymentResponse
            {
                Id = "fake_mollie_payment_id"
            };

            // Note: Since we're using a real PaymentClient, we won't mock CreatePaymentAsync method. 
            // The real PaymentClient should be able to handle this with a fake API key.

            // Act
            var result = await _paymentService.ProcessPaymentAsync(carId, garageId, paymentDate, fakeRedirectUrl);

            // Assert
            Assert.AreEqual(expectedFee, result.Price);
            Assert.AreEqual(carId, result.CarId);
            Assert.AreEqual(garageId, result.GarageId);
            Assert.AreEqual(CORE.Enums.PaymentType.Standard, result.Type);
            Assert.AreEqual(paymentDate, result.Date);
            Assert.AreEqual("fake_mollie_payment_id", result.ExternalPaymentId);

            // Verify that Create method of PaymentRepository was called exactly once
            _mockPaymentRepo.Verify(repo => repo.Create(It.IsAny<Payment>()), Times.Once);
            _mockPaymentRepo.Verify(repo => repo.Update(It.IsAny<Payment>()), Times.Once);
        }

        [TestMethod]
        public void GetPaymentsByAuth0Id_ReturnsPayments()
        {
            // Arrange
            string auth0Id = "auth0|123456";
            int accountId = 1;
            var account = new Account { Id = accountId };
            var cars = new List<Car>
            {
                new Car { Id = 1, AccountId = accountId },
                new Car { Id = 2, AccountId = accountId }
            };
            var payments = new List<Payment>
            {
                new Payment { CarId = 1, Price = 1000 },
                new Payment { CarId = 2, Price = 2000 }
            };

            _mockAccountRepo.Setup(repo => repo.GetAccountByAuth0Id(auth0Id)).Returns(account);
            _mockCarRepo.Setup(repo => repo.GetAllByAccountId(accountId)).Returns(cars);
            _mockPaymentRepo.Setup(repo => repo.GetPaymentsByCarIds(It.IsAny<List<int>>())).Returns(payments);

            // Act
            var result = _paymentService.GetPaymentsByAuth0Id(auth0Id);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1000, result[0].Price);
            Assert.AreEqual(2000, result[1].Price);
        }

        // Additional tests can be added here
    }
}
