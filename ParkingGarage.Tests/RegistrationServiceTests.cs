using CORE.Entities;
using CORE.Interfaces;
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
    public class RegistrationServiceTests
    {
        [TestMethod]
        public void CreateCar_NewCar_ShouldCreateSuccessfully()
        {
            // Arrange
            var mockRepository = new Mock<ICarRepository>();
            var service = new RegistrationService(mockRepository.Object);
            var accountId = 1;
            var licensePlate = "ABC123";

            // Act
            service.CreateCar(accountId, licensePlate);

            // Assert
            mockRepository.Verify(repo => repo.Create(It.IsAny<Car>()), Times.Once);
        }

    }
}
