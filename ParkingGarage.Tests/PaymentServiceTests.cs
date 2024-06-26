﻿using CORE.Entities;
using CORE.Interfaces.IRepositories;
using CORE.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingGarage.Tests
{
    [TestClass]
    public class PaymentServiceTests
    {
        private Mock<IPaymentRepository> _mockPaymentRepo;
        private Mock<ISpotOccupationRepository> _mockSpotOccupationRepo;
        private PaymentService _paymentService;
        private SpotOccupation _testSpotOccupation;
        private DateTime _entryTime;
        private DateTime _exitTime;

        [TestInitialize]
        public void SetUp()
        {
            // Mock setup
            _mockPaymentRepo = new Mock<IPaymentRepository>();
            _mockSpotOccupationRepo = new Mock<ISpotOccupationRepository>();

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

            // Initialize the service with the mocked repositories
            _paymentService = new PaymentService(_mockPaymentRepo.Object, _mockSpotOccupationRepo.Object);
        }

        [TestMethod]
        public void ProcessPayment_CalculatesCorrectFee()
        {
            // Arrange
            int ratePerHour = 300;
            int expectedFee = (int)((_exitTime - _entryTime).TotalHours * ratePerHour); // Expected fee in cents
            int carId = 1;
            int garageId = 1;
            DateTime paymentDate = DateTime.Now;

            // Act
            var result = _paymentService.ProcessPayment(carId, garageId, paymentDate);

            // Assert
            Assert.AreEqual(expectedFee, result.Price);
            Assert.AreEqual(carId, result.CarId);
            Assert.AreEqual(garageId, result.GarageId);
            Assert.AreEqual(CORE.Enums.PaymentType.Standard, result.Type);
            Assert.AreEqual(paymentDate, result.Date);

            // Verify that Create method of PaymentRepository was called exactly once
            _mockPaymentRepo.Verify(repo => repo.Create(It.IsAny<Payment>()), Times.Once);
        }
    }
}