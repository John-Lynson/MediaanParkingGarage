using CORE.Interfaces;
using CORE.Models;
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
    public class RegistrationServiceTests
    {
        [TestMethod]
        public void RegisterPlate_NewLicense_ShouldRegisterSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<ILicensePlateRepository>();
            mockRepository.Setup(repo => repo.GetLicensePlate(It.IsAny<string>())).Returns((LicensePlate)null);

            var service = new RegistrationService(mockRepository.Object);
            var newLicense = new LicensePlate { LicensePlateNumber = "ABC123" };

            // Act
            service.RegisterPlate(newLicense);

            // Assert
            mockRepository.Verify(repo => repo.AddLicensePlate(newLicense), Times.Once);
        }

    }
}
