using CORE.Entities;
using CORE.Interfaces.IRepositories;
using CORE.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ISpotOccupationRepository _spotOccupationRepository;
        private readonly IParkingSpotRepository _parkingSpotRepository;


        public ReservationService(ISpotOccupationRepository spotOccupationRepository, IParkingSpotRepository parkingSpotRepository)
        {
            _spotOccupationRepository = spotOccupationRepository;
            _parkingSpotRepository = parkingSpotRepository;
        }
        public int RetrieveOrReserveSpot(int carId, DateTime expectedStartDate, DateTime expectedEndDate)
        {
            // Check for existing reservation
            var existingReservation = _spotOccupationRepository.FindByCarIdAndDate(carId, expectedStartDate, expectedEndDate);
            if (existingReservation != null)
            {
                return existingReservation.ParkingSpotId; // Return the ID of the already reserved spot
            }

            // Attempt to find an available spot
            var allSpots = _parkingSpotRepository.GetAllParkingSpotIds(); 
            var availableSpots = _spotOccupationRepository.GetAvailableSpaces(allSpots, expectedStartDate, expectedEndDate);

            if (!availableSpots.Any())
            {
                throw new InvalidOperationException("No available spots.");
            }

            var spotId = availableSpots.First(); 
            ReserveSpots(carId, new List<int> { spotId }, expectedStartDate, expectedEndDate); 

            return spotId; 
        }
        public bool ReserveSpots(int carId, List<int> parkingSpotIds, DateTime expectedStartDate, DateTime expectedEndDate)
        {
            var availableSpots = _spotOccupationRepository.GetAvailableSpaces(parkingSpotIds, expectedStartDate, expectedEndDate);
            var unavailableSpots = parkingSpotIds.Except(availableSpots).ToList();

            if (unavailableSpots.Any())
            {
                // Throw an exception listing the unavailable spots
                var unavailableSpotsMessage = string.Join(", ", unavailableSpots);
                throw new InvalidOperationException($"The following spots are unavailable: {unavailableSpotsMessage}.");
            }

            foreach (var spotId in availableSpots)
            {
                var spotOccupation = new SpotOccupation
                {
                    ParkingSpotId = spotId,
                    CarId = carId, 
                    ExpectedStartDate = expectedStartDate,
                    ExpectedEndDate = expectedEndDate,
                    ActualStartDate = DateTime.MinValue, // Placeholder, to be updated when the spot is actually occupied.
                    ActualEndDate = DateTime.MinValue, // Placeholder, to be updated when the spot is vacated.
                };

                _spotOccupationRepository.Create(spotOccupation);
            }

            return true;
        }
    }
}
