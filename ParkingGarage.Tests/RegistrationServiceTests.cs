using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CORE.Interfaces.IRepositories;
using CORE.Entities;
using CORE.Services;
using System;

namespace ParkingGarage.Tests
{
    [TestClass]
    public class RegistrationServiceTests
    {
        private Mock<ICarRepository> _mockCarRepository;
        private RegistrationService _registrationService;

        [TestInitialize]
        public void SetUp()
        {
            // Mock setup
            _mockCarRepository = new Mock<ICarRepository>();

            // Service initialization
            _registrationService = new RegistrationService(_mockCarRepository.Object);
        }

        [TestMethod]
        public void CreateCar_ValidCar_ShouldInvokeCreateOnce()
        {
            // Arrange
            int accountId = 1;
            string licensePlate = "ABC123";

            // Act
            _registrationService.CreateCar(accountId, licensePlate);

            // Assert
            _mockCarRepository.Verify(repo => repo.Create(It.Is<Car>(car => car.AccountId == accountId && car.LicensePlate == licensePlate)), Times.Once());
        }

        [TestMethod]
        public void AddAdditionalLicensePlate_ValidCar_NotAlreadyRegistered_ShouldInvokeCreateOnce()
        {
            // Arrange
            int accountId = 1;
            string newLicensePlate = "DEF456";

            _mockCarRepository.Setup(repo => repo.FindByAccountIdAndLicensePlate(accountId, newLicensePlate))
                .Returns((Car)null); // Simulate car not existing

            // Act
            _registrationService.AddAdditionalLicensePlate(accountId, newLicensePlate);

            // Assert
            _mockCarRepository.Verify(repo => repo.Create(It.Is<Car>(car => car.AccountId == accountId && car.LicensePlate == newLicensePlate)), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddAdditionalLicensePlate_AlreadyRegistered_ShouldThrowInvalidOperationException()
        {
            // Arrange
            int accountId = 1;
            string existingLicensePlate = "GHI789";
            Car existingCar = new Car { AccountId = accountId, LicensePlate = existingLicensePlate };

            _mockCarRepository.Setup(repo => repo.FindByAccountIdAndLicensePlate(accountId, existingLicensePlate)).Returns(existingCar); // Simulate car already exists

            // Act
            _registrationService.AddAdditionalLicensePlate(accountId, existingLicensePlate);
            // No need for Assert here since we expect an exception
        }
    }
}
