using CORE.Entities;
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
    public class ReservationServiceTests
    {
        private Mock<ISpotOccupationRepository> _mockSpotOccupationRepository;
        private ReservationService _reservationService;

        [TestInitialize]
        public void SetUp()
        {
            _mockSpotOccupationRepository = new Mock<ISpotOccupationRepository>();
            _reservationService = new ReservationService(_mockSpotOccupationRepository.Object);
        }

        [TestMethod]
        public void ReserveSpots_AllSpotsAvailable_ShouldSucceed()
        {
            // Arrange
            var parkingSpotIds = new List<int> { 1, 2, 3 };
            var carId = 1;
            DateTime expectedStartDate = DateTime.Today;
            DateTime expectedEndDate = DateTime.Today.AddDays(1);

            _mockSpotOccupationRepository.Setup(repo => repo.GetAvailableSpaces(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                         .Returns(parkingSpotIds); // Simulating all spots are available

            // Act
            var result = _reservationService.ReserveSpots(carId, parkingSpotIds, expectedStartDate, expectedEndDate);

            // Assert
            Assert.IsTrue(result);
            _mockSpotOccupationRepository.Verify(repo => repo.Create(It.IsAny<SpotOccupation>()), Times.Exactly(parkingSpotIds.Count));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ReserveSpots_SomeSpotsUnavailable_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var parkingSpotIds = new List<int> { 1, 2, 3 };
            var carId = 1;
            DateTime expectedStartDate = DateTime.Today;
            DateTime expectedEndDate = DateTime.Today.AddDays(1);
            var availableSpots = new List<int> { 1, 3 }; // Simulating spots 1 and 3 are available, but 2 is not

            _mockSpotOccupationRepository.Setup(repo => repo.GetAvailableSpaces(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                         .Returns(availableSpots);

            // Act
            _reservationService.ReserveSpots(carId, parkingSpotIds, expectedStartDate, expectedEndDate);

            // Since an exception is expected, no need for Assert.IsTrue here
        }
    }
}
