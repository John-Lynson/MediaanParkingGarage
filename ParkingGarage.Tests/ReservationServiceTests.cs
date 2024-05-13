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
        private Mock<IParkingSpotRepository> _mockParkingSpotRepository;
        private ReservationService _reservationService;

        [TestInitialize]
        public void SetUp()
        {
            _mockSpotOccupationRepository = new Mock<ISpotOccupationRepository>();
            _mockParkingSpotRepository = new Mock<IParkingSpotRepository>();
            _reservationService = new ReservationService(_mockSpotOccupationRepository.Object, _mockParkingSpotRepository.Object);
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
            _reservationService.AttemptToReserveSpot(carId, parkingSpotIds, expectedStartDate, expectedEndDate);

            // Assert
            _mockSpotOccupationRepository.Verify(repo => repo.Create(It.IsAny<SpotOccupation>()), Times.Once);
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
            _reservationService.AttemptToReserveSpot(carId, parkingSpotIds, expectedStartDate, expectedEndDate);

            // Since an exception is expected, no need for Assert.IsTrue here 
        }

        [TestMethod]
        public void RetrieveOrReserveSpot_ExistingReservation_ShouldReturnExistingSpotId()
        {
            // Arrange
            var carId = 1;
            DateTime expectedStartDate = DateTime.Today;
            DateTime expectedEndDate = DateTime.Today.AddDays(1);
            var existingSpotId = 2; // Assuming spot 2 is already reserved for this car
            _mockSpotOccupationRepository.Setup(repo => repo.FindByCarIdAndDate(carId, expectedStartDate, expectedEndDate))
                                         .Returns(new SpotOccupation { ParkingSpotId = existingSpotId });

            // Act
            var resultSpotId = _reservationService.RetrieveOrReserveSpot(carId, expectedStartDate, expectedEndDate);

            // Assert
            Assert.AreEqual(existingSpotId, resultSpotId);
        }

        [TestMethod]
        public void RetrieveOrReserveSpot_NoExistingReservation_ShouldSuccessfullyReserveSpot()
        {
            // Arrange
            var carId = 1;
            DateTime expectedStartDate = DateTime.Today;
            DateTime expectedEndDate = DateTime.Today.AddDays(1);
            // Set up the parking spot IDs that exist in the system
            var allSpotIds = new List<int> { 1, 2, 3 };
            // Assume all spots are initially available
            var availableSpots = new List<int> { 1, 2, 3 };

            _mockSpotOccupationRepository.Setup(repo => repo.FindByCarIdAndDate(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                         .Returns((SpotOccupation)null);

            _mockParkingSpotRepository.Setup(repo => repo.GetAllParkingSpotIds())
                                      .Returns(allSpotIds);

            _mockSpotOccupationRepository.Setup(repo => repo.GetAvailableSpaces(It.IsAny<List<int>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                                         .Returns(availableSpots);

            // Act: Attempt to retrieve or reserve a spot
            var reservedSpotId = _reservationService.RetrieveOrReserveSpot(carId, expectedStartDate, expectedEndDate);

            // Assert
            // Verify a spot ID is returned and is within the list of available spots
            Assert.IsTrue(availableSpots.Contains(reservedSpotId), "The reserved spot ID should be one of the available spots.");

            // Verify that a SpotOccupation entity for the reserved spot is created
            _mockSpotOccupationRepository.Verify(repo => repo.Create(It.Is<SpotOccupation>(so => so.ParkingSpotId == reservedSpotId && so.CarId == carId)), Times.Once, "A new SpotOccupation should be created for the reserved spot.");
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RetrieveOrReserveSpot_NoAvailableSpots_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var carId = 1;
            var expectedStartDate = DateTime.Today;
            var expectedEndDate = DateTime.Today.AddDays(1);
            var allSpotIds = new List<int> { 1, 2, 3 };
            var availableSpots = new List<int>(); // No spots are available

            _mockParkingSpotRepository.Setup(repo => repo.GetAllParkingSpotIds()).Returns(allSpotIds);
            _mockSpotOccupationRepository.Setup(repo => repo.GetAvailableSpaces(allSpotIds, expectedStartDate, expectedEndDate))
                                         .Returns(availableSpots); // No available spots

            // Act
            _reservationService.RetrieveOrReserveSpot(carId, expectedStartDate, expectedEndDate);

            // Assert is handled by ExpectedException
        }

    }
}
