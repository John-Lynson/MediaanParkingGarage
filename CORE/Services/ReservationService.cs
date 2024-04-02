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

        public ReservationService(ISpotOccupationRepository spotOccupationRepository)
        {
            _spotOccupationRepository = spotOccupationRepository;
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
